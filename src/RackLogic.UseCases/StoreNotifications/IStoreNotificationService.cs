using RackLogic.UseCases.StoreNotifications.AppStore;
using RackLogic.UseCases.StoreNotifications.GooglePlay;

namespace RackLogic.UseCases.StoreNotifications;
public interface IStoreNotificationService
{
  Task<T?> DecodeJWTTokenAsync<T>(string token);
  Task<SignedTransactionInfoData?> VerifyAppleReceiptByTransactionIdAsync(string id);
  Task<T?> DecodeBase64Async<T>(string base64);
  Task<SubscriptionPurchase?> VerifyGooglePlayPurchaseTokenAsync(string? productId = "", string? token = "");
}
