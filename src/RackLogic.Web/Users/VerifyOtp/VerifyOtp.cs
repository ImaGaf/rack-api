using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Users.VerifyOtp;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.VerifyOtp;

public class VerifyOtp(IMediator _mediator) : Endpoint<VerifyOtpRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/users/verifyOtp");
    AllowAnonymous();
  }
  public override async Task HandleAsync(VerifyOtpRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new VerifyOtpCommand(req.Email, req.Otp));

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
