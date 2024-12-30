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
