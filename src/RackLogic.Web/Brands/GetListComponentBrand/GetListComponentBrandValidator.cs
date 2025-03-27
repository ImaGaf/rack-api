using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Brands.GetListComponentBrand;

public class GetListComponentBrandValidator : Validator<GetListComponentBrandRequest>
{
  public GetListComponentBrandValidator()
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
