using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.DeleteUserWarehouse;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Services;

namespace RackLogic.Web.Warehouses.DeleteUserWarehouse;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeleteUserWarehouse(IMediator _mediator) : Endpoint<DeleteUserWarehouseRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Delete("/manager-user/deleteUserWarehouse");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(DeleteUserWarehouseRequest req, CancellationToken ct)
  {
    var validator = new DeleteUserWarehouseValidator();
    var validationResult = validator.Validate(req);  

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var userCreatedId = User.GetUserId();
    var result = await _mediator.Send(new DeleteUserWarehouseCommand(
      userCreatedId,
      req.UserId, 
      req.WarehouseId
      ),
      ct);

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
