using Ardalis.Result;

namespace RackLogic.UseCases.StoreNotifications.AppStore;
public record AppStoreNotificationPayloadCommand(string SignedPayload)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;
