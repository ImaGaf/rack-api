namespace RackLogic.Web.BayConfigurator.ExportSWL;

public class ShareSWLRequest
{
  public Guid Id { get; set; }
  public string? Message { get; set; }
  public string? AseetName { get; set; }
  public string? RackNumber { get; set; }
  public required string Email { get; set; }
}
