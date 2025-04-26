using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.BayConfigurator;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.BayConfigurator.UpdateSWL;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UpdateSWL(IMediator _mediator) : Endpoint<UpdateSWLRequest, ApiResponse<BayConfigurationDTO>>
{
  public override void Configure()
  {
    Put("/bay-configurators/SWL/{Id}");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(UpdateSWLRequest req, CancellationToken ct)
  {
    var validator = new UpdateSWLRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(req.ToUpdateSWLCommand(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<BayConfigurationDTO>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
