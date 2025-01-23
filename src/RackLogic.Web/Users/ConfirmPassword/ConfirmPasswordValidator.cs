using FluentValidation;

namespace RackLogic.Web.Users.ConfirmPassword;

public class ConfirmPasswordValidator : AbstractValidator<ConfirmPasswordRequest>
{
  public ConfirmPasswordValidator()
  {
    RuleFor(x => x.Email)
        .NotEmpty()
        .EmailAddress()
        .WithMessage("Invalid email format.");

    RuleFor(x => x.OldPassword)
        .NotEmpty()
        .WithMessage("Old password is required.");
  }
}
