using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Users.Delete;
using RackLogic.Web.Exceptions;
using RackLogic.Web.FloorPlans.Create;
using RackLogic.Web.FloorPlans.Get;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.Delete;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class Delete(IMediator _mediator) : Endpoint<DeleteRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Delete("/users/delete");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
  {
    var validator = new DeleteValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new DeleteUserCommand(req.UserId, req.DeleteAccountToken), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
