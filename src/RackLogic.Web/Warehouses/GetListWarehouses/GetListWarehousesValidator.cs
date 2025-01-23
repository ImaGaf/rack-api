using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Warehouses.GetListWarehouses;

public class GetListWarehousesValidator : Validator<GetListWarehousesRequest>
{
  public GetListWarehousesValidator()
  {
    RuleFor(x => x.UserId)
   .NotEmpty().WithMessage("UserId is required.")
   .Must(BeValidGuid).WithMessage("UserId must be a valid GUID.");

  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
