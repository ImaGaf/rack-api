using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.FloorPlans.Get;
public class GetFloorPlanByIdHandler(IFloorPlanService _floorPlanService)
  : IQueryHandler<GetFloorPlanByIdQuery, Result<List<FloorPlanDTO>>>
{
  public async Task<Result<List<FloorPlanDTO>>> Handle(GetFloorPlanByIdQuery request, CancellationToken cancellationToken)
  {
    var floorPlans = await _floorPlanService.GetFloorPlansWithFilesByWarehouseIdAsync(request.WarehouseId);

    if (floorPlans == null)
    {
      return Result.Success(new List<FloorPlanDTO>());
    }

    return Result.Success(floorPlans);
  }
}
