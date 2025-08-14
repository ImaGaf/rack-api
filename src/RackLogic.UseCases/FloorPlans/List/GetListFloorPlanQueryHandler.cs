using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.FloorPlans.List;

public class GetListFloorPlanQueryHandler(
  IFloorPlanService _floorPlanService)
  : IQueryHandler<GetListFloorPlanQuery, Result<IEnumerable<FloorPlanSimpleDto>>>
{
  public async Task<Result<IEnumerable<FloorPlanSimpleDto>>> Handle(GetListFloorPlanQuery request, CancellationToken cancellationToken)
  {
    var floorPlans = await _floorPlanService.GetListAsync(request, cancellationToken);

    return Result.Success(floorPlans);
  }
}
