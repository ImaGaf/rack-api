using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Brands.GetListProductsBrand;

public class GetListProductsBrandValidator : Validator<GetListProductsBrandRequest>
{
  public GetListProductsBrandValidator()
  {
    RuleFor(x => x.BrandId)
        .Must(BeValidGuid).WithMessage("Invalid BrandId format.");
  }
  private bool BeValidGuid(Guid? guid)
  {
    if (guid is null) return true;

    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
