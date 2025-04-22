using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.FloorPlans.List;
using RackLogic.Web.Models;

namespace RackLogic.Web.DefectLogs.GetFilter;

public class GetListFloorPlan(IMediator _mediator)
  : Endpoint<GetListFloorPlanRequest, ApiResponse<IEnumerable<FloorPlanSimpleDto>>>
{
  public override void Configure()
  {
    Get("/defect-logs/filters");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(GetListFloorPlanRequest req, CancellationToken ct)
  {
    var floorPlan = await _mediator.Send(new GetListFloorPlanQuery(req.WarehouseId), ct);

    if (floorPlan.IsUnauthorized())
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    if (floorPlan.Value is null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    Response = new ApiResponse<IEnumerable<FloorPlanSimpleDto>>(floorPlan.Value);

    return;
  }
}
