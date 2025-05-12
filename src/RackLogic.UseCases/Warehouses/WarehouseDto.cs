namespace RackLogic.UseCases.Warehouses;

public class WarehouseDto
{
  public Guid? WarehouseId { get; set; }
  public string? Name { get; set; }
  public Guid? OwnerId { get; set; }
  public string? OwnerFirstName { get; set; }
  public string? OwnerLastName { get; set; }
  public string? OwnerEmailAddress { get; set; }
  public string? OwnerPhoneNumber { get; set; }
  public string? ABN { get; set; }
  public string? Address { get; set; }
  public string? AddressOptional { get; set; }
  public string? City { get; set; }
  public string? State { get; set; }
  public string? PostCode { get; set; }
  public bool? IsGoogleApiAddress { get; set; }
  public string? ClientPhoneNumber { get; set; }
  public string? FileImageName { get; set; }
  public string? FilePathImage { get; set; }
  public DateTime? InspectionDueDate { get; set; }
}
