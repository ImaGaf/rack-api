using Ardalis.Result;

namespace RackLogic.UseCases.StoreNotifications.GooglePlay;
public record GooglePlayNotificationCommand(
    string Data
) : Ardalis.SharedKernel.ICommand<Result<bool>>;
