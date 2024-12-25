using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.EditRoleUser;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.EditRoleUser;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EditRoleUser(IMediator _mediator) : Endpoint<EditRoleUserRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Put("/manager-user/editRoleUser");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(EditRoleUserRequest req, CancellationToken ct)
  {
    var validator = new EditRoleUserRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }
    var result = await _mediator.Send(new EditRoleUserCommand(User, req.UserId, req.WarehouseId, req.Role), ct);

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
