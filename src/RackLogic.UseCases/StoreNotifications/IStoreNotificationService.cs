using RackLogic.UseCases.StoreNotifications.AppStore;

namespace RackLogic.UseCases.StoreNotifications;
public interface IStoreNotificationService
{
  Task<T?> DecodeJWTTokenAsync<T>(string token);
  Task<SignedTransactionInfoData?> VerifyAppleReceiptByTransactionIdAsync(string id);
}
