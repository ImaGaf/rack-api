using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Brands.GetProductsByCategory;

public class GetProductsByCategoryValidator : Validator<GetProductsByCategoryRequest>
{
  public GetProductsByCategoryValidator()
  {
    RuleFor(x => x.ManufacturerId)
      .NotEmpty().WithMessage("ManufacturerId is required.")
      .Must(BeValidGuid).WithMessage("ManufacturerId must be a valid GUID.");
    RuleFor(x => x.Category)
      .NotEmpty().WithMessage("Category is required.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
