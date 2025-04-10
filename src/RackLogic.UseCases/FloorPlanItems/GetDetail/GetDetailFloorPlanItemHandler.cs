using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlanItems.GetDetail;

public class GetDetailFloorPlanItemHandler(IFloorPlanItemService _floorPlanItemService)
  : IQueryHandler<GetDetailFloorPlanItemQuery, Result<FloorPlanItemDto>>
{
  public async Task<Result<FloorPlanItemDto>> Handle(GetDetailFloorPlanItemQuery request, CancellationToken cancellationToken)
  {
    var floorPlanItem = await _floorPlanItemService.GetDetailAsync(request, cancellationToken);
      
    if (floorPlanItem == null) return Result.Error("FloorPlanItem is not existed");

    return Result.Success(floorPlanItem);
  }
}
