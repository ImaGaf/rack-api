namespace RackLogic.UseCases.Warehouses;

public record WarehouseProfileDTO
{
  public Guid Id { get; set; }
  public Guid OwnerId { get; set; }
  public string? CompanyName { get; set; }
  public string? ABN { get; set; }
  public string? SiteAddress { get; set; }
  public string? ClientPhoneNumber { get; set; }
  public string? FileImageName { get; set; }
  public string? FileImageUrl { get; set; }
  public string? OwnerFirstName { get; set; }
  public string? OwnerLastName { get; set; }
  public string? OwnerEmailAddress { get; set; }
  public string? OwnerPhoneNumber { get; set; }
}
