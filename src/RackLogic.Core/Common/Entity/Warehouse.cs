using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class Warehouse : BaseEntityCustom
{
  public required string CompanyName { get; set; }
  public string? ABN { get; set; }
  public required string Address { get; set; }
  public string? AddressOptional { get; set; }
  public string? City { get; set; }
  public string? State { get; set; }
  public string? PostCode { get; set; }
  public bool? IsGoogleApiAddress { get; set; }
  public required string ClientPhoneNumber { get; set; }
  public string? FileImageName { get; set; }
  public string? FileImageUrl { get; set; }
  public required string OwnerFirstName { get; set; }
  public required string OwnerLastName { get; set; }
  public string? OwnerEmailAddress { get; set; }
  public string? OwnerPhoneNumber { get; set; }
  public DateTime? InspectionDueDate { get; set; }
  public Guid? OwnerId { get; set; }
  public virtual ApplicationUser? User { get; set; }
  public virtual required ICollection<FloorPlan> FloorPlans { get; set; }
  public virtual required ICollection<UserWarehouseRole> UserWarehouseRoles { get; set; }
  public virtual ICollection<OEMBrand> OEMBrands { get; set; } = new List<OEMBrand>();
  public virtual ICollection<BayConfiguration> Configurations { get; set; } = new List<BayConfiguration>();
  public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
