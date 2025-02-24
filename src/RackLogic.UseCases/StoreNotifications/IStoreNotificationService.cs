using RackLogic.UseCases.StoreNotifications.AppStore;

namespace RackLogic.UseCases.StoreNotifications;
public interface IStoreNotificationService
{
  Task<TransactionInfo?> DecodeJWTToken(string token);
}
