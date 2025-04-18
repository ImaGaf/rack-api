using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace RackLogic.UseCases.FloorPlans.Detail;

public class GetDetailFloorPlanQueryHandler : IQueryHandler<GetDetailFloorPlanQuery, Result<FloorPlanDTO>>
{
  private readonly IFloorPlanService _floorPlanService;
  public GetDetailFloorPlanQueryHandler(IFloorPlanService floorPlanService)
  {
    _floorPlanService = floorPlanService;
  }

  async Task<Result<FloorPlanDTO>> IRequestHandler<GetDetailFloorPlanQuery, Result<FloorPlanDTO>>.Handle(GetDetailFloorPlanQuery request, CancellationToken cancellationToken)
  {
    var floorPlan = await _floorPlanService.GetDetailAsync(request.wareHouseId, request.floorPlanId, cancellationToken);

    if (floorPlan is null)
      return Result<FloorPlanDTO>.NotFound();

    return Result.Success(floorPlan);
  }
}
