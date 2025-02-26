using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.ConfirmReceipt.AppStore;
public record ConfirmReceiptCommand(
  Guid UserId,
  string TransactionId)
  : ICommand<Result<bool>>;
