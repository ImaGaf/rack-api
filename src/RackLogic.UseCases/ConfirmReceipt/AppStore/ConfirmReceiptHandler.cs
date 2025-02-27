using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.StoreNotifications;
using RackLogic.UseCases.StoreNotifications.AppStore;

namespace RackLogic.UseCases.ConfirmReceipt.AppStore;
public class ConfirmReceiptHandler(
  UserManager<ApplicationUser> _userManager,
  IRepositoryWithExpression<PurchaseRecord> _purchaseRecordRepository,
  IStoreNotificationService _storeNotificationService)
  : ICommandHandler<ConfirmReceiptCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(ConfirmReceiptCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByIdAsync(request.UserId.ToString());
    if (user == null) return Result.Invalid(new ValidationError("User not found."));
    // Confirm transaction by AppStore API.
    var signedTransactionInfoData = await _storeNotificationService.VerifyAppleReceiptByTransactionId(request.TransactionId);
    // Decode transaction token by response.
    var transactionInfo = await _storeNotificationService.DecodeJWTToken<TransactionInfo>(signedTransactionInfoData?.SignedTransactionInfo ?? string.Empty);
    if (transactionInfo is null) return Result.Invalid(new ValidationError("Invalid transaction code."));

    if (await TransactionIdExistsAsync(request.TransactionId)) return Result.Invalid(new ValidationError("The transaction already exists."));

    return await CreatePurchaseRecordAsync(transactionInfo, user.Id, signedTransactionInfoData?.SignedTransactionInfo,cancellationToken);
  }

  private async Task<bool> TransactionIdExistsAsync(string transactionId)
  {
    var result = await _purchaseRecordRepository.FirstOrDefaultAsync(x => x.PurchaseToken == transactionId);
    return result is not null;
  }

  private async Task<bool> CreatePurchaseRecordAsync(TransactionInfo transactionInfo, Guid userId,string? receiptData, CancellationToken cancellationToken)
  {
    var entity = new PurchaseRecord()
    {
      UserId = userId,
      PurchaseToken = transactionInfo.TransactionId,
      OrderId = transactionInfo.OriginalTransactionId,
      ProductId = transactionInfo.ProductId,
      PurchaseTime = DateTimeOffset.FromUnixTimeMilliseconds(transactionInfo.PurchaseDate).UtcDateTime,
      ExpiryTime = DateTimeOffset.FromUnixTimeMilliseconds(transactionInfo.ExpiresDate).UtcDateTime,
      Status = transactionInfo.TransactionReason,
      AutoRenewing = !string.IsNullOrEmpty(transactionInfo.SubscriptionGroupIdentifier),
      IsRefunded = transactionInfo.TransactionReason == "REFUND",
      ReceiptData = receiptData ?? string.Empty,
      CreationTime = DateTime.UtcNow
    };

    var result = await _purchaseRecordRepository.AddAsync(entity, cancellationToken);
    if (result is null) return false;

    return true;
  }
}
