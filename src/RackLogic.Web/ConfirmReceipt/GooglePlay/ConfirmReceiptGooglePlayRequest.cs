namespace RackLogic.Web.ConfirmReceipt.GooglePlay;

public class ConfirmReceiptGooglePlayRequest
{
  public Guid UserId { get; set; }
  public required string ProductId { get; set; }
  public required string PurchaseToken { get; set; }
}
