using RackLogic.Infrastructure.Token;
using RackLogic.UseCases.StoreNotifications;
using RackLogic.UseCases.StoreNotifications.AppStore;

namespace RackLogic.Infrastructure.Data.Queries;
public class StoreNotificationService : IStoreNotificationService
{
  public async Task<TransactionInfo?> DecodeJWTToken(string token)
  {
    var notification = await TokenUtil.DecodeJwt<AppStoreNotification>(token);
    if (notification is null || notification.Data == null) return new TransactionInfo();

    var transactionInfo = await TokenUtil.DecodeJwt<TransactionInfo>(notification.Data.SignedTransactionInfo);
    return transactionInfo;
  }
}
