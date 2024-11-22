using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Users.ResetPassword;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Users.Register;

namespace RackLogic.Web.Users.ResetPassword;

public class ResetPassword(IMediator _mediator) : Endpoint<ResetPasswordRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/users/resetPassword");
    AllowAnonymous();
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
  {
    var validator = new ResetPasswordRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new ResetPasswordCommand(req.Email, req.NewPassword, req.ResetPasswordToken), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    foreach (var error in result.Errors)
    {
      AddError(error);
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
