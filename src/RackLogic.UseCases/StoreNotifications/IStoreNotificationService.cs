using RackLogic.UseCases.StoreNotifications.AppStore;

namespace RackLogic.UseCases.StoreNotifications;
public interface IStoreNotificationService
{
  Task<T?> DecodeJWTToken<T>(string token);
  Task<SignedTransactionInfoData?> VerifyAppleReceiptByTransactionId(string id);
}
