using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Warehouses.WarehouseFinalUse;

public class WarehouseFinalUseValidator : Validator<WarehouseFinalUseRequest>
{
  public WarehouseFinalUseValidator()
  {
    RuleFor(x => x.WarehouseId)
    .NotEmpty().WithMessage("WarehouseId is required.")
    .Must(BeValidGuid).WithMessage("WarehouseId must be a valid GUID.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
