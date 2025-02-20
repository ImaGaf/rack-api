using System.Dynamic;
using System.Text.Json;
using FastEndpoints;

namespace RackLogic.Web.StoreNotifications.AppStore;

public class AppStoreNotifications(ILogger<AppStoreNotifications> logger) : Endpoint<ExpandoObject>
{
  public override void Configure()
  {
    Post("/AppStore/notifications");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ExpandoObject req, CancellationToken ct)
  {
    string requestData = JsonSerializer.Serialize(req, new JsonSerializerOptions { WriteIndented = true });
    logger.LogInformation($"Log Appstore req: {requestData}");
    Response = true;

    await SendAsync(true, cancellation: ct);
  }
}
