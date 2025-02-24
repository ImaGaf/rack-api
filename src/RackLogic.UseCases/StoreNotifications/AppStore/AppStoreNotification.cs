namespace RackLogic.UseCases.StoreNotifications.AppStore;
public class AppStoreNotification
{
  public string NotificationType { get; set; } = string.Empty;

  public string NotificationUUID { get; set; } = string.Empty;

  public AppStoreNotificationData? Data { get; set; }

  public string Version { get; set; } = string.Empty;

  public long SignedDate { get; set; }
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

public class TransactionInfo
{
  public string TransactionId { get; set; } = string.Empty;
  public string OriginalTransactionId { get; set; } = string.Empty;
  public string WebOrderLineItemId { get; set; } = string.Empty;
  public string BundleId { get; set; } = string.Empty;
  public string ProductId { get; set; } = string.Empty;
  public string SubscriptionGroupIdentifier { get; set; } = string.Empty;
  public long PurchaseDate { get; set; }
  public long OriginalPurchaseDate { get; set; }
  public long ExpiresDate { get; set; }
  public int Quantity { get; set; }
  public string Type { get; set; } = string.Empty;
  public string InAppOwnershipType { get; set; } = string.Empty;
  public long SignedDate { get; set; }
  public string Environment { get; set; } = string.Empty;
  public string TransactionReason { get; set; } = string.Empty;
  public string Storefront { get; set; } = string.Empty;
  public string StorefrontId { get; set; } = string.Empty;
  public long Price { get; set; }
  public string Currency { get; set; } = string.Empty;
}
