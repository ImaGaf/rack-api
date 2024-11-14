using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Users.ResetPassword;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.ResetPassword;

public class ResetPassword(IMediator _mediator) : Endpoint<ResetPasswordRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/users/resetPassword");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new ResetPasswordCommand(req.Email, req.NewPassword), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      AddError(result.ValidationErrors?.FirstOrDefault()?.ErrorMessage ?? string.Empty);
      await SendErrorsAsync(400, ct);
      return;
    }

    foreach (var error in result.Errors)
    {
      AddError(error);
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
