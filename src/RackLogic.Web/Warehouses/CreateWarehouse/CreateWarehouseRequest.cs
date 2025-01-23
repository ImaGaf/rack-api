using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Warehouses.CreateWarehouse;

public class CreateWarehouseRequest
{
  [Required]
  public required string CompanyName { get; set; }
  [Required]
  public required string ABN { get; set; }
  [Required]
  public required string SiteAddress { get; set; }
  [Required]
  public required string ClientPhoneNumber { get; set; }
  [Required]
  public required IFormFile FileImageLogo { get; set; }
  [Required]
  public required string OwnerFirstName { get; set; }
  [Required]
  public required string OwnerLastName { get; set; }
  [Required]
  public required string OwnerPhoneNumber { get; set; }
  [Required]
  public required string OwnerEmailAddress { get; set; }
  public bool IsNominatedInspector { get; set; }
  public string? InviteInspectorEmail { get; set; }
}
