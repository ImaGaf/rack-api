using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.FloorPlanItems;
using RackLogic.UseCases.FloorPlanItems.GetDetail;
using RackLogic.UseCases.FloorPlanItems.Inspection;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;

public class FloorPlanItemService : IFloorPlanItemService
{
  private readonly AppDbContext _context;

  public FloorPlanItemService(AppDbContext context)
  {
    _context = context;
  }


  public async Task<FloorPlanItemDto?> GetDetailAsync(GetDetailFloorPlanItemQuery query, CancellationToken cancellationToken = default)
  {
    var floorPlanItem = await _context.FloorPlanItems
                                    .Include(x => x.Inspection)
                                    .Include(x => x.BayConfiguration!).ThenInclude(b => b.BayConfigurationLevels)
                                    .Include(x => x.BayConfiguration!).ThenInclude(b => b.Components)
                                    .Include(x => x.DefectLogs!)
                                      .ThenInclude(x => x.Component)
                                    .Include(x => x.DefectLogs!)
                                      .ThenInclude(x => x.Component)
                                    .FirstOrDefaultAsync(x => x.Id == query.floorPlanItemId, cancellationToken);

    return floorPlanItem != null ? floorPlanItem.Mapping() : null;
  }

  public async Task<PagedDataDTO<InspectionFloorPlanItemDto>> GetListInspectionItemAsync(InspectionFloorPlanItemQuery request, CancellationToken cancellationToken)
  {
    var totalItem = await _context.FloorPlanItems.CountAsync(x => x.FloorPlan!.WarehouseId == request.WarehouseId);

    var items = await _context.FloorPlanItems
                            .Include(x => x.FloorPlan!)
                              .ThenInclude(f => f.Warehouse!.Configurations)
                            .Where(x => x.FloorPlan!.WarehouseId == request.WarehouseId)
                            .Skip(((request.pageNumber - 1) * request.pageSize))
                            .Take(request.pageSize)
                            .OrderByDescending(x => x.DueDate)
                            .ToListAsync();

    return new PagedDataDTO<InspectionFloorPlanItemDto>()
    {
      TotalRecords = totalItem,
      PageNumber = request.pageNumber <= 0 ? PaginationDefaultValue.PageNumber : request.pageNumber,
      PageSize = request.pageSize <= 0 ? PaginationDefaultValue.PageSize : request.pageSize,
      DataDTO = items.Select(x => x.ToInspectionFloorPlanItemDto()).ToList()
    };
  }

  public async Task<bool> UpdateInspectionDueDate(Guid floorPlanItemId)
  {
    var item = Guard.Against.Null(
      await _context.FloorPlanItems
        .Include(f => f.BayConfiguration)
        .Include(f => f.FloorPlan)
        .FirstOrDefaultAsync(f => f.Id == floorPlanItemId)
      );

    var warehouseId = item.FloorPlan!.WarehouseId;
    var warehouse = Guard.Against.Null(await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == warehouseId));

    item.DueDate = DateTime.UtcNow.AddDays(item.BayConfiguration!.InterimInterval);
    warehouse.InspectionDueDate = DateTime.UtcNow.AddYears(1);

    _context.Warehouses.Update(warehouse);
    _context.FloorPlanItems.Update(item);

    await _context.SaveChangesAsync();

    return true;
  }
}
