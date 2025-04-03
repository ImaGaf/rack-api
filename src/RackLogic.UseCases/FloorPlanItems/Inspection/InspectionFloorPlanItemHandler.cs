using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlanItems.Inspection;

public class InspectionFloorPlanItemHandler(
  IFloorPlanItemService _floorPlanItemService)
  : IQueryHandler<InspectionFloorPlanItemQuery, Result<PagedDataDTO<InspectionFloorPlanItemDto>>>
{
  public async Task<Result<PagedDataDTO<InspectionFloorPlanItemDto>>> Handle(InspectionFloorPlanItemQuery request, CancellationToken cancellationToken)
  {
    var items = await _floorPlanItemService.GetListInspectionItem(request);

    return Result.Success(items);
  }
}
