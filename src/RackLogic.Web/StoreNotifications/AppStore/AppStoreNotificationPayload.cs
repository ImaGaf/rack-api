namespace RackLogic.Web.StoreNotifications.AppStore;

public class AppStoreNotificationPayload
{
  public string NotificationType { get; set; } = string.Empty;
  public string Subtype { get; set; } = string.Empty;
  public string NotificationUUID { get; set; } = string.Empty;
  public AppStoreNotificationData Data { get; set; } = new AppStoreNotificationData();
  public string Version { get; set; } = string.Empty;
}

public class AppStoreNotificationData
{
  public long AppAppleId { get; set; }
  public string BundleId { get; set; } = string.Empty;
  public string BundleVersion { get; set; } = string.Empty;
  public string Environment { get; set; } = string.Empty;
  public string SignedTransactionInfo { get; set; } = string.Empty;
  public string SignedRenewalInfo { get; set; } = string.Empty;
  public int Status { get; set; }
}

public class TestAPI
{

}
