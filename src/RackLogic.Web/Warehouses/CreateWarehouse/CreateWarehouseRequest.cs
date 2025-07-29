using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Warehouses.CreateWarehouse;

public class CreateWarehouseRequest
{
  [Required]
  public required string CompanyName { get; set; }
  public string? ABN { get; set; }
  [Required]
  public required string Address { get; set; }
  public string? AddressOptional { get; set; }
  [Required]
  public required string City { get; set; }
  [Required]
  public required string State { get; set; }
  [Required]
  public required string PostCode { get; set; }
  public bool? IsGoogleApiAddress { get; set; }
  [Required]
  public required string ClientPhoneNumber { get; set; }
  [Required]
  public required IFormFile FileImageLogo { get; set; }
  [Required]
  public required string OwnerFirstName { get; set; }
  [Required]
  public required string OwnerLastName { get; set; }

  public string? OwnerPhoneNumber { get; set; }

  public string? OwnerEmailAddress { get; set; }
  public bool IsNominatedInspector { get; set; }
  public string? InviteInspectorEmail { get; set; }
}
