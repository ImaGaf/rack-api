using FastEndpoints;
using MediatR;
using RackLogic.UseCases.StoreNotifications.AppStore;

namespace RackLogic.Web.StoreNotifications.AppStore;

public class AppStoreNotifications(
  IMediator _mediator) 
  : Endpoint<AppStoreNotificationPayload>
{
  public override void Configure()
  {
    Post("/AppStore/notifications");
    AllowAnonymous();
  }

  public override async Task HandleAsync(AppStoreNotificationPayload req, CancellationToken ct)
  {
    var result = await _mediator.Send(new AppStoreNotificationPayloadCommand(req.SignedPayload), ct);

    if (result.IsSuccess)
    {
      Response = result.Value;
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
