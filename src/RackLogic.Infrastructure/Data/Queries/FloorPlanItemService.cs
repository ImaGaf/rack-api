using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.FloorPlanItems;
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

  public async Task<PagedDataDTO<InspectionFloorPlanItemDto>> GetListInspectionItem(InspectionFloorPlanItemQuery request)
  {
    var totalItem = await _context.FloorPlanItems.CountAsync(x => x.FloorPlan!.WarehouseId == request.WarehouseId);

    var items = await _context.FloorPlanItems
                            .Include(x => x.FloorPlan!)
                              .ThenInclude(f => f.Warehouse!.Configurations)
                            .Where(x => x.FloorPlan!.WarehouseId == request.WarehouseId)
                            .Skip(((request.pageNumber - 1) * request.pageSize))
                            .Take(request.pageSize)

                            // TODO: Need property DueDate to sor Decsending
                            //.OrderByDescending(x => x.DueDate)
                            .ToListAsync();

    return new PagedDataDTO<InspectionFloorPlanItemDto>()
    {
      TotalRecords = totalItem,
      PageNumber = request.pageNumber <= 0 ? PaginationDefaultValue.PageNumber : request.pageNumber,
      PageSize = request.pageSize <= 0 ? PaginationDefaultValue.PageSize : request.pageSize,
      DataDTO = items.Select(x => x.ToInspectionFloorPlanItemDto()).ToList()
    };
  }
}
