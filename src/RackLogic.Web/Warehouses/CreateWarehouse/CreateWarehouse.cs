using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RackLogic.Core.Common.Constants;
using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.CreateWarehouse;

public class CreateWarehouse(IMediator _mediator) : Endpoint<CreateWarehouseRequest, ApiResponse<WarehouseDto>>
{
  public override void Configure()
  {
    Post("/manager-warehouse/createWarehouse");
    AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    Roles(RoleConstant.Inspector, RoleConstant.Premises);
    DontThrowIfValidationFails();
    AllowFileUploads();
  }

  public override async Task HandleAsync(CreateWarehouseRequest req, CancellationToken ct)
  {
    var validator = new CreateWarehouseValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(req.ToCreateWarehouseCommand(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<WarehouseDto>(result.Value);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync();
      return;
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    if (result.IsError())
    {
      throw new ArgumentException(result?.Errors?.FirstOrDefault());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
