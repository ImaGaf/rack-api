using FastEndpoints;
using MediatR;
using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.FloorPlans.Detail;
using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlans.Detail;

public class DetailFloorPlan(IMediator _mediator) : Endpoint<DetailFloorPlanRequest, ApiResponse<FloorPlanDTO>>
{
  public override void Configure()
  {
    Get("/floor-plan/asset/detail");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(DetailFloorPlanRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetDetailFloorPlanQuery(req.wareHouserId, req.floorPlanId), ct);
    if (result.IsSuccess)
    {
      Response = new ApiResponse<FloorPlanDTO>(result.Value);
    }
  }
}
