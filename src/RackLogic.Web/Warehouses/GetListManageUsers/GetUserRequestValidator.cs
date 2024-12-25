using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Warehouses.GetListManageUsers;

public class GetUserRequestValidator : Validator<GetManageUserRequest>
{
  public GetUserRequestValidator()
  {
    RuleFor(x => x.WareHouseId)
            .NotEmpty().WithMessage("WarehouseId is required.")
            .Must(BeValidGuid).WithMessage("Invalid WarehouseId format.");

    RuleFor(x => x.PageNumber)
           .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1.");

    RuleFor(x => x.PageSize)
        .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
