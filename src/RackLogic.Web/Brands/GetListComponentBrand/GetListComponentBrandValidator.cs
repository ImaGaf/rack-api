using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Brands.GetListComponentBrand;

public class GetListComponentBrandValidator : Validator<GetListComponentBrandRequest>
{
  public GetListComponentBrandValidator()
  {
    RuleFor(x => x.BrandId)
        .NotEmpty().WithMessage("BrandId is required.")
        .Must(BeValidGuid).WithMessage("Invalid BrandId format.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
