using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.ConfirmReceipt.AppStore;

public class ConfirmReceiptRequestValidator : Validator<ConfirmReceiptRequest>
{
  public ConfirmReceiptRequestValidator()
  {
    RuleFor(x => x.UserId)
     .NotEmpty().WithMessage("UserId is required.")
     .Must(BeValidGuid).WithMessage("UserId must be a valid GUID.");
    RuleFor(x => x.TransactionId)
      .NotEmpty().WithMessage("TransactionId can be not null.");
  }

  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
