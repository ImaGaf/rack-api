using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Auth.Login;

public class LoginRequestValidator : Validator<LoginRequest>
{
  public LoginRequestValidator()
  {
    RuleFor(r => r.Email)
     .EmailAddress()
     .WithMessage("Email is invalid.");

    RuleFor(r => r.Password)
      .NotEmpty()
      .WithMessage("Invalid password format.");
  }
}
