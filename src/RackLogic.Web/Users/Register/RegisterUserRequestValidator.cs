using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Users.Register;

public class RegisterUserRequestValidator : Validator<RegisterUserRequest>
{
  public RegisterUserRequestValidator()
  {
    RuleFor(r => r.EmailAddress)
      .EmailAddress()
      .WithMessage("Email is invalid.");

    RuleFor(r => r.Password)
      .NotEmpty()
      .Length(8, 20)
      .Matches(@"[A-Z]")
      .Matches(@"[a-z]")
      .Matches(@"\d")
      .WithMessage("Invalid password format.");

    RuleFor(r => r.PhoneNumber)
      .NotEmpty()
      .Matches(@"^0[45]\d{8}$")
      .WithMessage("Invalid phone number format.");

    RuleFor(r => r.FirstName)
      .NotEmpty()
      .WithMessage("Fist name is required.");

    RuleFor(r => r.LastName)
      .NotEmpty()
      .WithMessage("Last name is required.");

    RuleFor(r => r.Postcode)
      .NotEmpty()
      .MaximumLength(4)
      .WithMessage("Invalid post code format.");

    RuleFor(r => r.Role)
      .NotEmpty()
      .WithMessage("Role is required");
  }
}
