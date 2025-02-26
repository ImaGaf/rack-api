using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.StoreNotifications.AppStore;
public class AppStoreNotificationsHandler(
  IRepositoryWithExpression<PurchaseRecord> _purchaseRecordRepository,
  IStoreNotificationService _storeNotificationService) 
  : ICommandHandler<AppStoreNotificationPayloadCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(AppStoreNotificationPayloadCommand request, CancellationToken cancellationToken)
  {
    // Get transaction Id
    var transactionInfoByRequest = await GetTransactionInfoByToken(request.SignedPayload);
    // Confirm transaction with Appstore api
    var signedTransactionInfoData = await  _storeNotificationService.VerifyAppleReceiptByTransactionId(transactionInfoByRequest?.TransactionId ?? string.Empty);
    // Decode transaction token by Appstore api response
    var transactionInfo = await _storeNotificationService.DecodeJWTToken<TransactionInfo>(signedTransactionInfoData?.SignedTransactionInfo ?? string.Empty);
    if (transactionInfo is null) return false;

    return await UpdateTransaction(transactionInfo, request.SignedPayload, cancellationToken);
  }

  private async Task<TransactionInfo?> GetTransactionInfoByToken(string token)
  {
    var appStoreNotification = await _storeNotificationService.DecodeJWTToken<AppStoreNotification>(token);
    if (appStoreNotification is null) return null;

    return await _storeNotificationService.DecodeJWTToken<TransactionInfo>(appStoreNotification.Data?.SignedTransactionInfo ?? string.Empty);
  }

  private async Task<bool> UpdateTransaction(TransactionInfo transactionInfo, string receiptData, CancellationToken cancellationToken)
  {
    var entity = await _purchaseRecordRepository.FirstOrDefaultAsync(x => x.OrderId == transactionInfo.OriginalTransactionId);
    if (entity is null) return false;

    entity.PurchaseToken = transactionInfo.TransactionId;
    entity.OrderId = transactionInfo.OriginalTransactionId;
    entity.ProductId = transactionInfo.ProductId;
    entity.PurchaseTime = DateTimeOffset.FromUnixTimeMilliseconds(transactionInfo.PurchaseDate).UtcDateTime;
    entity.ExpiryTime = DateTimeOffset.FromUnixTimeMilliseconds(transactionInfo.ExpiresDate).UtcDateTime;
    entity.Status = transactionInfo.TransactionReason;
    entity.AutoRenewing = !string.IsNullOrEmpty(transactionInfo.SubscriptionGroupIdentifier);
    entity.IsRefunded = transactionInfo.TransactionReason == "REFUND";
    entity.ReceiptData = receiptData;
    entity.LastModificationTime = DateTime.UtcNow;
    await _purchaseRecordRepository.UpdateAsync(entity,cancellationToken);
    return true;
  }
}
