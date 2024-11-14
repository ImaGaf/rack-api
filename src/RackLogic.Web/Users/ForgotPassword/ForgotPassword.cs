using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using RackLogic.UseCases.Users.ForgotPassword;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.ForgotPassword;

public class ForgotPassword(IMediator _mediator) : Endpoint<ForgotPasswordRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/users/forgotPassword");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new ForgotPasswordCommand(req.Email), ct);

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
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
