namespace RackLogic.Web.StoreNotifications.GooglePlay;

public class GooglePlayNotificationRequest
{
  public required GooglePlayMessage Message { get; set; }
  public required string Subscription { get; set; }
}


public class GooglePlayMessage
{
  public required string Data { get; set; }
  public required string MessageId { get; set; }
  public DateTime publishTime { get; set; }
}
