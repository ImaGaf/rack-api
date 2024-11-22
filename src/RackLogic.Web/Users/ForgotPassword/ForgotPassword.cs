using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using RackLogic.UseCases.Users.ForgotPassword;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.ForgotPassword;

public class ForgotPassword(IMediator _mediator) : Endpoint<ForgotPasswordRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/users/forgotPassword");
    AllowAnonymous();
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
  {
    var validator = new ForgotPasswordRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new ForgotPasswordCommand(req.Email), ct);

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
