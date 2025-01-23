using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.WarehouseFinalUse;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.WarehouseFinalUse;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class WarehouseFinalUse(IMediator _mediator) : Endpoint<WarehouseFinalUseRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Put("/manager-warehouse/warehouse-final-use");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(WarehouseFinalUseRequest req, CancellationToken ct)
  {
    var validator = new WarehouseFinalUseValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new WarehouseFinalUseCommand(User, req.WarehouseId), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync();
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
