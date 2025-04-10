using FastEndpoints;
using MediatR;
using RackLogic.UseCases.FloorPlanItems.GetDetail;
using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlanItems.GetDetail;

public class GetDetailFloorPlanItem(IMediator _mediator) : Endpoint<GetDetailFloorPlanItemRequest, ApiResponse<FloorPlanItemDto>>
{
  public override void Configure()
  {
    Get("/floor-plan-items/detail");
  }

  public override async Task HandleAsync(GetDetailFloorPlanItemRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetDetailFloorPlanItemQuery(req.floorPlanItemId), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<FloorPlanItemDto>(result);

      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
