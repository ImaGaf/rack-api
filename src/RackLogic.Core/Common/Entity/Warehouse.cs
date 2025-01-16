using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class Warehouse : BaseEntityCustom
{
  public required string CompanyName { get; set; }
  public required Guid OwnerId { get; set; }
  public ApplicationUser? User { get; set; }
  public required ICollection<UserWarehouseRole> UserWarehouseRoles { get; set; }
  public required ICollection<FloorPlan> FloorPlans { get; set; }
}
