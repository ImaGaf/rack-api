using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.UseCases.Warehouses;
using RackLogic.UseCases.Inspections.Update;
using RackLogic.UseCases.Inspections.End;
using Ardalis.Result;

namespace RackLogic.Infrastructure.Data.Queries;

public class InspectionService(AppDbContext dbContext) : IInspectionService
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

    if (request.IsConfirmAuthorization && !itemsConfirmed!.Contains(request.FloorPlanItemId.ToString()))
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
}
