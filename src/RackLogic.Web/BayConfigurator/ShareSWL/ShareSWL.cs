using FastEndpoints;
using MediatR;
using RackLogic.UseCases.BayConfigurator.ShareSWL;
using RackLogic.Web.Models;

namespace RackLogic.Web.BayConfigurator.ExportSWL;

public class ShareSWL(IMediator _mediator) : Endpoint<ShareSWLRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/bay-configurators/SWL/send");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(ShareSWLRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new ShareSWLCommand(req.Id, req.Message, req.AseetName, req.RackNumber, req.Email), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(true);
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
