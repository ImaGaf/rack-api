using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Warehouses.GetListManageUsers;

public class GetUserRequestValidator : Validator<GetManageUserRequest>
{
  public GetUserRequestValidator()
  {
    RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("WarehouseId is required.")
            .Must(BeValidGuid).WithMessage("Invalid WarehouseId format.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
