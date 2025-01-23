using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Users.Delete;

public class DeleteValidator : Validator<DeleteRequest>
{
  public DeleteValidator()
  {
    RuleFor(x => x.UserId)
        .NotEmpty().WithMessage("UserId is required.")
        .Must(BeValidGuid).WithMessage("Invalid UserId format.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
