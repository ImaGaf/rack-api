using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.FloorPlanItems;
using RackLogic.UseCases.FloorPlans.DetailDefectLog;
using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlans.DetailDefectLog;

public class DetailDefectLogFloorPlan(IMediator _mediator) : Endpoint<DetailDefectLogFloorPlanRequest, ApiResponse<IEnumerable<FloorPlanItemDto>>>
{
  public override void Configure()
  {
    Get("/floor-plan/detail-defect-log");
    DontThrowIfValidationFails();
  }
  public override async Task HandleAsync(DetailDefectLogFloorPlanRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetDetailDefectLogQuery(req.floorPlanId), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<IEnumerable<FloorPlanItemDto>>(result.Value);
    }
  }
}
