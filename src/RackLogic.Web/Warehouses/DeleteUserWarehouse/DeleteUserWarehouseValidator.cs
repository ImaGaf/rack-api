using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Warehouses.DeleteUserWarehouse;

public class DeleteUserWarehouseValidator : Validator<DeleteUserWarehouseRequest>
{
  public DeleteUserWarehouseValidator()
  {
    RuleFor(x => x.UserId)
    .NotEmpty().WithMessage("UserId is required.")
    .Must(BeValidGuid).WithMessage("UserId must be a valid GUID.");

    RuleFor(x => x.UserId)
    .NotEmpty().WithMessage("WarehouseId is required.")
    .Must(BeValidGuid).WithMessage("WarehouseId must be a valid GUID.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
