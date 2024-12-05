using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.InviteUser;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.InviteUser;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InviteUser(IMediator _mediator) : Endpoint<InviteUserRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/manager-user/invite-user");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(InviteUserRequest req, CancellationToken ct)
  {
    var validator = new InviteUserRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new InviteUserCommand(User, req.Email, req.WarehouseId, req.Role), ct);

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync();
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
