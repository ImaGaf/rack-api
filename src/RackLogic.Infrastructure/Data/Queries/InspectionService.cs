using System.Text;
using Ardalis.GuardClauses;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Inspections.ExportReport;
using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.UseCases.Inspections.Update;
using RackLogic.UseCases.Inspections.End;
using Ardalis.Result;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;

public class InspectionService(
  AppDbContext dbContext,
  IConverter converter) : IInspectionService
{
  public async Task<Result> EndAsync(EndInspectionCommand request, CancellationToken cancellationToken)
  {
    var inspection = await dbContext.Inspections.FirstOrDefaultAsync(x => x.Id == request.Id);

    if (inspection is null)
      return Result.NotFound();

    inspection.IsEnd = request.IsEnd;

    dbContext.Inspections.Update(inspection);

    await dbContext.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }

  public async Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    var inspection = Guard.Against.Null(await dbContext.Inspections
      .Where(i => i.Id == id)
      .Include(i => i.Warehouse)
      .Include(i => i.AuthorisePerson)
      .FirstOrDefaultAsync(cancellationToken));

    var result = inspection!.ToDto();

    result.AuthorisedPersonName = inspection.AuthorisePerson?.FirstName + " " + inspection.AuthorisePerson?.LastName;

    return result;
  }

  public async Task<PagedDataDTO<InspectionReportDto>> SearchReportAsync(SearchInspectionReportQuery request, Guid userId, CancellationToken cancellationToken)
  {
    var userWarehouseIds = await dbContext.UserWarehouseRoles
      .Where(uwr => uwr.UserId == userId)
      .Select(uwr => uwr.WarehouseId)
      .ToListAsync(cancellationToken);

    var query = dbContext.Inspections
      .Include(i => i.Warehouse)
      .Include(i => i.AuthorisePerson!)
        .ThenInclude(i => i.InspectorProfile)
      .Where(i => i.IsEnd == true && userWarehouseIds.Contains(i.WarehouseId!.Value));

    if (request.Search != null)
    {
      var search = request.Search;
      query = query.Where(i => i.Warehouse!.CompanyName!.Contains(search) ||
                               (i.AuthorisePerson!.FirstName + " " + i.AuthorisePerson.LastName).Contains(search));
    }

    var totalItem = await query.CountAsync(cancellationToken);

    var result = await query
      .Skip((request.PageNumber - 1) * request.PageSize)
      .Take(request.PageSize)
      .Select(i => i.ToReportDtoWithFullName())
      .ToListAsync(cancellationToken);

    return new PagedDataDTO<InspectionReportDto>
    {
      TotalRecords = totalItem,
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      DataDTO = result
    };
  }

  public async Task<InspectionDto?> UpdateAsync(UpdateInspectionCommand request, CancellationToken cancellationToken)
  {
    var inspection = await dbContext.Inspections
      .Include(d => d.FloorPlan!.FloorPlanItems)
      .FirstOrDefaultAsync(d => d.Id == request.InspectionId, cancellationToken);

    if (inspection == null)
      return null;

    var itemsConfirmed = inspection.FloorPlanItemInspection?.Split(',').ToList();

    if (itemsConfirmed is null)
      itemsConfirmed = new List<string>();

    if (request.IsConfirmAuthorization && !itemsConfirmed.Contains(request.FloorPlanItemId.ToString()))
      inspection.FloorPlanItemInspection += !string.IsNullOrEmpty(inspection.FloorPlanItemInspection)
                                            ? $",{request.FloorPlanItemId.ToString()}"
                                            : request.FloorPlanItemId.ToString();

    else if (itemsConfirmed!.Contains(request.FloorPlanItemId.ToString()) && !request.IsConfirmAuthorization)
    {
      itemsConfirmed.Remove(request.FloorPlanItemId.ToString());
      inspection.FloorPlanItemInspection = string.Join(",", itemsConfirmed);
    }

    dbContext.Inspections.Update(inspection);
    await dbContext.SaveChangesAsync(cancellationToken);

    return inspection.ToDto();
  }

  public async Task<byte[]> ExportReportPdfAsync(ExportReportCommand command, CancellationToken cancellationToken)
  {
    var inspection = await dbContext.Inspections
      .Where(i => i.Id == command.InspectionId)
      .Include(i => i.Warehouse)
      .Include(i => i.AuthorisePerson!)
        .ThenInclude(i => i.InspectorProfile)
      .FirstOrDefaultAsync(cancellationToken);

    if (inspection == null) return Array.Empty<byte>();

    var templatePath = Path.Combine("wwwroot", "ReportTemplates", "OverviewReport.html");
    var html = await File.ReadAllTextAsync(templatePath, cancellationToken);

    html = html.Replace("{{Name}}", inspection.Warehouse!.CompanyName) 
      .Replace("{{Address}}", inspection.Warehouse?.Address ?? "-")
      .Replace("{{InspectionCompany}}", inspection.AuthorisePerson?.InspectorProfile?.CompanyName ?? "N/A")
      .Replace("{{InspectorName}}", inspection.AuthorisePerson?.FirstName + " " + inspection.AuthorisePerson?.LastName)
      .Replace("{{InspectionDate}}", inspection.InspectionDate.ToString("yyyy-MM-dd"))
      .Replace("{{NextInspectionDate}}", inspection.NextInspectionDate.ToString("yyyy-MM-dd"));

    var defectLogs = await dbContext.DefectLogs
      .Where(d => d.InspectionId == command.InspectionId)
      .Include(d => d.FloorPlanItem)
      .Include(d => d.Component!)
        .ThenInclude(c => c.BayConfiguration!)
          .ThenInclude(b => b.Manufacturer)
      .Include(d => d.DefectLogFiles)
      .AsSplitQuery()
      .ToListAsync(cancellationToken);

    var reportDefects = defectLogs.Select(d => d.ToReportDto()).ToList();

    var defectLogsHtml = new StringBuilder();

    defectLogsHtml.AppendLine(@"
      <table style='width:100%; border-collapse:collapse; font-size:14px;'>
        <thead>
          <tr style='background-color:#F4F4F4; text-align:left;'>
            <th style='padding:8px; border:1px solid #ccc;'>Defect Grade</th>
            <th style='padding:8px; border:1px solid #ccc;'>Defect ID</th>
            <th style='padding:8px; border:1px solid #ccc;'>Bay ID</th>
            <th style='padding:8px; border:1px solid #ccc;'>Component</th>
            <th style='padding:8px; border:1px solid #ccc;'>OEM</th>
            <th style='padding:8px; border:1px solid #ccc;'>Part Number</th>
            <th style='padding:8px; border:1px solid #ccc;'>Defect Type</th>
            <th style='padding:8px; border:1px solid #ccc;'>Corrective Action</th>
            <th style='padding:8px; border:1px solid #ccc;'>Image(s)</th>
            <th style='padding:8px; border:1px solid #ccc;'>Comments</th>
          </tr>
        </thead>
        <tbody>
      ");

    foreach (var defect in reportDefects)
    {
      var hasImages = (defect.DefectLogFiles != null && defect.DefectLogFiles.Any()) ? "Yes" : "No";
      var grade = defect.DefectGrade?.ToString();

      var color = grade switch
      {
        "Severe" => "#FF0000",   // Red
        "Moderate" => "#FF6B00", // Orange
        "Slight" => "#0A89FF",   // Blue
        _ => "#4F4F4F"           // Grey
      };

      var gradeDot = $"<span style='display:inline-block; width:14px; height:14px; border-radius:50%; background-color:{color};'></span>";

      defectLogsHtml.AppendLine("<tr>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc; text-align:center;'>{gradeDot}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.TicketId ?? "-"}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.Rack}.{defect.Bay}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.Component?.ComponentType ?? "-"}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.OEMBrand?.Name ?? "-"}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.Component?.Code ?? "-"}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.DefectType?.ToString() ?? "-"}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.CorrectiveAction?.ToString() ?? "-"}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{hasImages}</td>");
      defectLogsHtml.AppendLine($"<td style='padding:8px; border:1px solid #ccc;'>{defect.Comment ?? "-"}</td>");
      defectLogsHtml.AppendLine("</tr>");
    }

    defectLogsHtml.AppendLine("</tbody></table>");

    html = html.Replace("{{DefectLogsSection}}", defectLogsHtml.ToString());

    var doc = new HtmlToPdfDocument()
    {
      GlobalSettings = new GlobalSettings
      {
        PaperSize = PaperKind.A4,
        Orientation = Orientation.Landscape,
        Margins = new MarginSettings { Top = 0, Bottom = 0 },
        DocumentTitle = "Inspection Report"
      },
      Objects = {
        new ObjectSettings
        {
          HtmlContent = html, // your main HTML content
          WebSettings = { DefaultEncoding = "utf-8" },
          FooterSettings = new FooterSettings
          {
            HtmUrl = Path.Combine("wwwroot", "ReportTemplates", "Footer.html"),
            Right = "[page]",
            FontSize = 10,
            FontName = "Arial",
            Line = false,
            Spacing = 0,
          }
        }
      }
    };

    var pdfBytes = converter.Convert(doc);
    return pdfBytes;
  }
}
