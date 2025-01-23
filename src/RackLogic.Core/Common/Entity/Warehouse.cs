using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class Warehouse : BaseEntityCustom
{
  public required string CompanyName { get; set; }
  public required Guid OwnerId { get; set; }
  public ApplicationUser? User { get; set; }
  public required string ABN { get; set; }
  public required string SiteAddress { get; set; }
  public required string ClientPhoneNumber { get; set; }
  public string? FileImageName { get; set; }
  public string? FileImageUrl { get; set; }
  public required string OwnerFirstName { get; set; }
  public required string OwnerLastName { get; set; }
  public required string OwnerEmailAddress { get; set; }
  public required string OwnerPhoneNumber { get; set; }
  public required ICollection<UserWarehouseRole> UserWarehouseRoles { get; set; }
  public required ICollection<FloorPlan> FloorPlans { get; set; }
}
