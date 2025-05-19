using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Inspections.End;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.End;

public class EndInspection(IMediator _mediator) : Endpoint<EndInspectionRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Put("/inspections/end");
  }

  public override async Task HandleAsync(EndInspectionRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new EndInspectionCommand(req.Id, req.IsEnd), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(true);
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
