using System.Text;
using Ardalis.GuardClauses;
using Ardalis.Result;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RackLogic.UseCases.Common.Extensions;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Inspections.End;
using RackLogic.UseCases.Inspections.ExportReport;
using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.UseCases.Inspections.Update;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;

public class InspectionService(
  AppDbContext dbContext,
  IConfiguration configuration,
  IConverter converter) : IInspectionService
{
  public async Task<Result> EndAsync(EndInspectionCommand request, CancellationToken cancellationToken)
  {
    var inspection = await dbContext.Inspections.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

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
    var html = await GetReportHtmlAsync(command.InspectionId, cancellationToken, true);

    var doc = new HtmlToPdfDocument()
    {
      GlobalSettings = new GlobalSettings
      {
        PaperSize = PaperKind.A4,
        Orientation = Orientation.Portrait,
        DocumentTitle = "Inspection Report",
        Margins = new MarginSettings { Top = 25, Bottom = 20 }
      },
      Objects = {
        new ObjectSettings
        {
          HtmlContent = html,
          WebSettings = { DefaultEncoding = "utf-8" },
          HeaderSettings = new HeaderSettings
          {
            HtmUrl = Path.Combine("wwwroot", "ReportTemplates", "Header.html")
          },
          FooterSettings = new FooterSettings
          {
            Center = "[page]",
            FontSize = 8,
            Spacing = 5
          }
        }
      }
    };

    var pdfBytes = converter.Convert(doc);
    return pdfBytes;
  }

  public async Task<string> GetReportHtmlAsync(Guid inspectionId, CancellationToken cancellationToken, bool hasDefectLog = false)
  {
    var inspection = await dbContext.Inspections
      .Where(i => i.Id == inspectionId)
      .Include(i => i.Warehouse)
      .Include(i => i.AuthorisePerson!)
      .ThenInclude(i => i.InspectorProfile)
      .Include(i => i.FloorPlan)
      .FirstOrDefaultAsync(cancellationToken);

    var templatePath = Path.Combine("wwwroot", "ReportTemplates", "OverviewReport.html");

    if (hasDefectLog)
    {
      templatePath = Path.Combine("wwwroot", "ReportTemplates", "DetailedReport.html");
    }

    var html = await File.ReadAllTextAsync(templatePath, cancellationToken);

    var baseUrl = configuration.GetSection("Client:BaseUrl").Value!;
    var standard = configuration.GetSection("Client:Standard").Value!;

    var rackLogicLogo = $"{baseUrl}{configuration.GetSection("Client:LogoReport").Value}";
    string warehouseLogo = Path.Combine(baseUrl, inspection!.Warehouse?.FileImageUrl ?? "");
    string inspectorCompanyLogo = Path.Combine(baseUrl, inspection.AuthorisePerson?.InspectorProfile?.FileImageUrl ?? "");

    html = html.Replace("{{Name}}", inspection.Warehouse!.CompanyName)
      .Replace("{{AssetName}}", inspection.FloorPlan!.AssetName)
      .Replace("{{Standard}}", standard)
      .Replace("{{Address}}", inspection.Warehouse?.Address ?? "-")
      .Replace("{{InspectionCompany}}", inspection.AuthorisePerson?.InspectorProfile?.CompanyName ?? "-")
      .Replace("{{InspectorName}}", inspection.AuthorisePerson?.FirstName + " " + inspection.AuthorisePerson?.LastName)
      .Replace("{{InspectionDate}}", inspection.InspectionDate.ToString("yyyy-MM-dd"))
      .Replace("{{NextInspectionDate}}", inspection.NextInspectionDate.ToString("yyyy-MM-dd"))
      .Replace("{{WarehouseLogo}}", warehouseLogo)
      .Replace("{{InspectorCompanyLogo}}", inspectorCompanyLogo)
      .Replace("{{RackLogicLogo}}", rackLogicLogo);

    var defectLogs = await dbContext.DefectLogs
      .Where(d => d.InspectionId == inspectionId)
      .Include(d => d.FloorPlanItem)
      .Include(d => d.Component!)
        .ThenInclude(c => c.BayConfiguration!)
          .ThenInclude(b => b.Manufacturer)
      .Include(d => d.DefectLogFiles)
      .AsSplitQuery()
      .ToListAsync(cancellationToken);

    var reportDefects = defectLogs.Select(d => d.ToReportDto()).ToList();

    var defectLogRows = new StringBuilder();

    foreach (var defect in reportDefects)
    {
      var hasImages = (defect.DefectLogFiles != null && defect.DefectLogFiles.Any()) ? "Yes" : "No";
      var grade = defect.DefectGrade?.ToString();

      var color = grade switch
      {
        "Severe" => "#E14747",
        "Moderate" => "#E58A00",
        "Slight" => "#2CA87F",
        _ => "#4F4F4F"
      };

      var gradeDot = $"<span style='display:inline-block; width:14px; height:14px; border-radius:50%; background-color:{color};'></span>";

      defectLogRows.AppendLine("<tr>");
      defectLogRows.AppendLine($"<td>{gradeDot}</td>");
      defectLogRows.AppendLine($"<td>{defect.TicketId ?? "-"}</td>");
      defectLogRows.AppendLine($"<td>{defect.Bay}.{defect.Rack}</td>");
      defectLogRows.AppendLine($"<td>{defect.Component?.ComponentType ?? "-"}</td>");
      defectLogRows.AppendLine($"<td>{defect.OEMBrand?.Name ?? "-"}</td>");
      defectLogRows.AppendLine($"<td>{defect.Component?.Code ?? "-"}</td>");
      defectLogRows.AppendLine($"<td>{defect.DefectType?.GetDisplayName() ?? "-"}</td>");
      defectLogRows.AppendLine($"<td>{defect.CorrectiveAction?.ToString() ?? "-"}</td>");
      defectLogRows.AppendLine($"<td>{hasImages}</td>");
      defectLogRows.AppendLine($"<td>{defect.Comment ?? "-"}</td>");
      defectLogRows.AppendLine("</tr>");
    }

    html = html.Replace("{{DefectLogRows}}", defectLogRows.ToString());

    return html;
  }

  public async Task<IEnumerable<InspectionDto>> GetAllAsync(Guid warehouseId, CancellationToken cancellationToken)
  {
    var inspections = await dbContext.Inspections.Include(x => x.Warehouse).Where(x => x.WarehouseId == warehouseId).ToListAsync();

    var results = inspections.Select(x => x.ToDto()).ToList();

    return results;
  }
}
