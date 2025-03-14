using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.ConfirmReceipt.GooglePlay;
public record ConfirmReceiptGooglePlayCommand(
  Guid UserId,
  string ProductId,
  string PurchaseToken)
  : ICommand<Result<bool>>;
