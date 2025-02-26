using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class PurchaseRecord : BaseEntityCustom
{
  public required Guid UserId { get; set; }
  public ApplicationUser? User { get; set; }
  public required string PurchaseToken { get; set; } // transactionInfo.TransactionId
  public required string OrderId { get; set; } // same OriginalTransactionId in IOS
  public required string ProductId { get; set; }
  public DateTime PurchaseTime { get; set; }
  public DateTime? ExpiryTime { get; set; }
  public required string Status { get; set; }
  public bool AutoRenewing { get; set; }
  public bool IsRefunded { get; set; }
  public required string ReceiptData { get; set; }
}
