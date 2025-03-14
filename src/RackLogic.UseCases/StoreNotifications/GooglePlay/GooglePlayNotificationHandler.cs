using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.StoreNotifications.GooglePlay;
public class GooglePlayNotificationHandler(
  IRepositoryWithExpression<PurchaseRecord> _purchaseRecordRepository,
  IStoreNotificationService _storeNotificationService)
  : ICommandHandler<GooglePlayNotificationCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(GooglePlayNotificationCommand request, CancellationToken cancellationToken)
  {
    var result = await _storeNotificationService.DecodeBase64Async<GooglePlayNotification>(request.Data);
    var subscriptionPurchase = await _storeNotificationService.VerifyGooglePlayPurchaseTokenAsync(
      result?.SubscriptionNotification.SubscriptionId,
      result?.SubscriptionNotification.PurchaseToken);
    
    if (subscriptionPurchase is
      null) return false;

    return await UpdateTransactionAsync(subscriptionPurchase,
      result?.SubscriptionNotification.PurchaseToken ?? string.Empty,
      request.Data,
      result?.SubscriptionNotification.SubscriptionId ?? string.Empty,
      result?.SubscriptionNotification.NotificationType.ToString() ?? string.Empty,
      cancellationToken);
  }

  private async Task<bool> UpdateTransactionAsync(SubscriptionPurchase transactionInfo, string purchaseToken, string receiptData, string productId, string notificationType, CancellationToken cancellationToken)
  {
    var entity = await _purchaseRecordRepository.FirstOrDefaultAsync(x => x.OrderId == transactionInfo.OrderId);
    if (entity is null) return false;

    entity.PurchaseToken = purchaseToken;
    entity.OrderId = transactionInfo.OrderId;
    entity.ProductId = productId;
    entity.PurchaseTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(transactionInfo.StartTimeMillis)).UtcDateTime;
    entity.ExpiryTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(transactionInfo.ExpiryTimeMillis)).UtcDateTime;
    entity.Status = notificationType;
    entity.AutoRenewing = transactionInfo.AutoRenewing;
    entity.IsRefunded = transactionInfo.CancelReason == 1;
    entity.ReceiptData = receiptData;
    entity.LastModificationTime = DateTime.UtcNow;
    await _purchaseRecordRepository.UpdateAsync(entity, cancellationToken);
    return true;
  }
}
