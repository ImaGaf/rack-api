namespace RackLogic.UseCases.StoreNotifications;
public interface IStoreNotificationService
{
  Task<T?> DecodeJWTToken<T>(string token);
  Task<bool> VerifyAppleReceiptByTransactionId(string id);
}
