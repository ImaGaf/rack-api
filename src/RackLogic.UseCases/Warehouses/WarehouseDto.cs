namespace RackLogic.UseCases.Warehouses;

public class WarehouseDto
{
  public Guid? Id { get; set; }
  public string? CompanyName { get; set; }
  public Guid? OwnerId { get; set; }
  public string? ABN { get; set; }
  public string? SiteAddress { get; set; }
  public string? ClientPhoneNumber { get; set; }
  public string? FileImageName { get; set; }
  public string? FileImageUrl { get; set; }
  public DateTime? InspectionDueDate { get; set; }
}
