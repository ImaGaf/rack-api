using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Warehouses.UpdateWarehouse;

public class UpdateWarehouseRequest
{
  public Guid WarehouseId { get; set; }
  [Required]
  public required string CompanyName { get; set; }
  [Required]
  public required string ABN { get; set; }
  [Required]
  public string? Address { get; set; }
  public string? AddressOptional { get; set; }
  [Required]
  public string? City { get; set; }
  [Required]
  public string? State { get; set; }
  [Required]
  public string? PostCode { get; set; }
  public bool? IsGoogleApiAddress { get; set; }
  [Required]
  public required string ClientPhoneNumber { get; set; }
  [Required]
  public IFormFile? FileImageLogo { get; set; }
  [Required]
  public required string OwnerFirstName { get; set; }
  [Required]
  public required string OwnerLastName { get; set; }
  [Required]
  public required string OwnerPhoneNumber { get; set; }
  [Required]
  public required string OwnerEmailAddress { get; set; }
}
