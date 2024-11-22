using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Users.VerifyOtp;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.VerifyOtp;

public class VerifyOtp(IMediator _mediator) : Endpoint<VerifyOtpRequest, ApiResponse<VerifyOtpResponse>>
{
  public override void Configure()
  {
    Post("/users/verifyOtp");
    AllowAnonymous();
    DontThrowIfValidationFails();
  }
  public override async Task HandleAsync(VerifyOtpRequest req, CancellationToken ct)
  {
    var validator = new VerifyOtpRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new VerifyOtpCommand(req.Email, req.Otp));

    if (result.IsSuccess)
    {
      Response = new ApiResponse<VerifyOtpResponse>(new VerifyOtpResponse() { ResetPasswordToken = result.Value});
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
