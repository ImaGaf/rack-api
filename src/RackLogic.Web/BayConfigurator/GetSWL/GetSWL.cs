using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.BayConfigurator.GetSWL;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.BayConfigurator.GetSWL;

public class GetSWL(IMediator _mediator) : Endpoint<GetSWLRequest, ApiResponse<SWLSignageDTO>>
{
  public override void Configure()
  {
    Get("/bay-configurators/SWL/{BayConfigurationId}");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(GetSWLRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(req.ToGetSWLCommand(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<SWLSignageDTO>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
