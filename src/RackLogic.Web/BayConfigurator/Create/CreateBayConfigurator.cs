using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.BayConfigurator.Create;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Services;

namespace RackLogic.Web.BayConfigurator.Create;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateBayConfigurator(IMediator _mediator) : Endpoint<CreateBayConfiguratorRequest, bool>
{
  public override void Configure()
  {
    Post("/bay-configurators");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(CreateBayConfiguratorRequest req, CancellationToken ct)
  {
    var validator = new CreateBayConfiguratorRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(
      new CreateBayConfigurationCommand(
        req.BayConfigurator.ToCreateBayConfigurationCommand(),
        req.WarehouseId,
        User.GetUserId()),
      ct);

    if (result.IsForbidden())
    {
      await SendForbiddenAsync();
      return;
    }

    if (result.IsSuccess)
    {
      Response = result.Value;
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
