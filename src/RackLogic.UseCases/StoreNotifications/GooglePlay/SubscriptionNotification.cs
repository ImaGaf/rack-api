namespace RackLogic.UseCases.StoreNotifications.GooglePlay;
public class GooglePlayNotification
{
  public required string Version { get; set; }
  public required string PackageName { get; set; }
  public required string EventTimeMillis { get; set; }
  public required SubscriptionNotificationDetail SubscriptionNotification { get; set; }
}

public class SubscriptionNotificationDetail
{
  public string? Version { get; set; }
  public int NotificationType { get; set; }
  public required string PurchaseToken { get; set; }
  public required string SubscriptionId { get; set; }
}

public class SubscriptionPurchase
{
  public required string StartTimeMillis { get; set; }
  public required string ExpiryTimeMillis { get; set; }
  public bool AutoRenewing { get; set; }
  public required string PriceCurrencyCode { get; set; }
  public required string PriceAmountMicros { get; set; }
  public required string CountryCode { get; set; }
  public required string DeveloperPayload { get; set; }
  public int CancelReason { get; set; }
  public required string OrderId { get; set; }
  public int PurchaseType { get; set; }
  public int AcknowledgementState { get; set; }
  public required string Kind { get; set; }
}
