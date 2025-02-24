using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.StoreNotifications.AppStore;
public class AppStoreNotificationsHandler(IStoreNotificationService _storeNotificationService) 
  : ICommandHandler<AppStoreNotificationPayloadCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(AppStoreNotificationPayloadCommand request, CancellationToken cancellationToken)
  {
    var transactionInfo = await _storeNotificationService.DecodeJWTToken(request.SignedPayload);

    if (transactionInfo is null) return false;


    return true;
  }
}
