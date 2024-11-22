using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Users.ResetPassword;

public class ResetPasswordRequestValidator : Validator<ResetPasswordRequest>
{
  public ResetPasswordRequestValidator()
  {
    RuleFor(r => r.Email)
      .EmailAddress()
      .WithMessage("Email is invalid.");

    RuleFor(r => r.NewPassword)
      .NotEmpty()
      .Length(8, 20)
      .Matches(@"[A-Z]")
      .Matches(@"[a-z]")
      .Matches(@"\d")
      .WithMessage("Invalid password format.");
  }
}
