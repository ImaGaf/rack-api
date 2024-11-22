using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace RackLogic.Web.Users.ForgotPassword;

public class ForgotPasswordRequestValidator : Validator<ForgotPasswordRequest>
{
  public ForgotPasswordRequestValidator()
  {
    RuleFor(r => r.Email)
      .EmailAddress()
      .WithMessage("Email is invalid.");
  }
}
