namespace RackLogic.Web.ConfirmReceipt.AppStore;

public class ConfirmReceiptRequest
{
  public required Guid UserId { get; set; }
  public required string TransactionId { get; set; }
}
