using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using RackLogic.UseCases.FloorPlanItems.GetDetail;

namespace RackLogic.UseCases.FloorPlans.DetailDefectLog;

public class GetDetailDefectLogQueryHandler : IQueryHandler<GetDetailDefectLogQuery, Result<IEnumerable<FloorPlanItemDto>>>
{
  private readonly IFloorPlanService _floorPlanService;

  public GetDetailDefectLogQueryHandler(IFloorPlanService floorPlanService)
  {
    _floorPlanService = floorPlanService;
  }

  async Task<Result<IEnumerable<FloorPlanItemDto>>> IRequestHandler<GetDetailDefectLogQuery, Result<IEnumerable<FloorPlanItemDto>>>.Handle(GetDetailDefectLogQuery request, CancellationToken cancellationToken)
  {
    var floorPlan = await _floorPlanService.GetDetailDefectLogAsync(request.floorPlanId, cancellationToken);

    if (floorPlan is null)
      return Result<IEnumerable<FloorPlanItemDto>>.NotFound();

    return Result.Success(floorPlan);
  }
}
