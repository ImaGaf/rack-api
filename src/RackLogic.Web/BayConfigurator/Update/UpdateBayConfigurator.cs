using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.BayConfigurator.Update;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Services;

namespace RackLogic.Web.BayConfigurator.Update;

public class UpdateBayConfigurator(IMediator _mediator) : Endpoint<UpdateBayConfiguratorRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Put("/bay-configurators");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(UpdateBayConfiguratorRequest req, CancellationToken ct)
  {
    var validator = new UpdateBayconfiguratorRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(
      new UpdateBayConfigurationCommand(
        req.BayConfigurator.ToUpdateBayConfigurationCommand(),
        req.WarehouseId,
        User.GetUserId()),
      ct);

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
