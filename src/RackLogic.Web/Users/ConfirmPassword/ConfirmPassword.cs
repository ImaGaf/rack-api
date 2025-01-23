using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Users.ConfirmPassword;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.ConfirmPassword;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class ConfirmPassword(IMediator _mediator) : Endpoint<ConfirmPasswordRequest, ApiResponse<string>>
{
  public override void Configure()
  {
    Post("/users/confirmPassword");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(ConfirmPasswordRequest req, CancellationToken ct)
  {
    var validator = new ConfirmPasswordValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new ConfirmPasswordCommand(req.Email, req.OldPassword), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<string>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
