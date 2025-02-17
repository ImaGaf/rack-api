using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class WarehouseFinalUses : BaseEntityCustom
{
  public required Guid UserId { get; set; }
  public ApplicationUser? User { get; set; }
  public required Guid WarehouseId { get; set; }
  public Warehouse? Warehouse { get; set; } 
}
