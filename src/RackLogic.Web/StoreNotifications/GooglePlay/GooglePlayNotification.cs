using FastEndpoints;
using MediatR;
using RackLogic.UseCases.StoreNotifications.GooglePlay;

namespace RackLogic.Web.StoreNotifications.GooglePlay;

public class GooglePlayNotification(IMediator _mediator, IConfiguration configuration)
  : Endpoint<GooglePlayNotificationRequest>
{
  public override void Configure()
  {
    Post(configuration["Endpoints:GooglePlayNotificationPath"] ?? string.Empty);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GooglePlayNotificationRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GooglePlayNotificationCommand(req.Message.Data));

    if (result.IsSuccess)
    {
      Response = result.Value;
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
