using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Warehouses.InviteUser;

public class InviteUserRequestValidator : Validator<InviteUserRequest>
{
  public InviteUserRequestValidator()
  {
    RuleFor(x => x.InvitedById)
            .NotEmpty().WithMessage("InvitedById is required.")
            .Must(BeValidGuid).WithMessage("InvitedById must be a valid GUID.");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email must be a valid email address.");

    RuleFor(x => x.WarehouseId)
        .NotEmpty().WithMessage("WarehouseId is required.")
        .Must(BeValidGuid).WithMessage("WarehouseId must be a valid GUID.");

    RuleFor(x => x.Role)
        .NotEmpty().WithMessage("Role is required.");
  }

  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
