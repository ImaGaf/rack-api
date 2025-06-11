namespace RackLogic.Web.BayConfigurator.ExportSWL;

public class ShareSWLRequest
{
  public Guid Id { get; set; }
  public string? Message { get; set; }
  public Guid AssetId { get; set; }
  public int? RackNumber { get; set; }
  public required string Email { get; set; }
}
