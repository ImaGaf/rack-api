using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.ConfirmReceipt.GooglePlay;

public class ConfirmReceiptGooglePlayRequestValidator : Validator<ConfirmReceiptGooglePlayRequest>
{
  public ConfirmReceiptGooglePlayRequestValidator()
  {
    RuleFor(x => x.UserId)
     .NotEmpty().WithMessage("UserId is required.")
     .Must(BeValidGuid).WithMessage("UserId must be a valid GUID.");
    RuleFor(x => x.ProductId)
      .NotEmpty().WithMessage("PackageName is required.");
    RuleFor(x => x.PurchaseToken)
      .NotEmpty().WithMessage("PurchaseToken is required.");
  }

  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
