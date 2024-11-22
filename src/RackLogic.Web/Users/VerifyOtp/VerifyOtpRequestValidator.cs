using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Users.VerifyOtp;

public class VerifyOtpRequestValidator : Validator<VerifyOtpRequest>
{
  public VerifyOtpRequestValidator()
  {
    RuleFor(r => r.Email)
      .EmailAddress()
      .WithMessage("Email is invalid.");
  }
}
