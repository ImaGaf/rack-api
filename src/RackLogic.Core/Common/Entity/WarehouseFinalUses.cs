using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class WarehouseFinalUses : BaseEntityCustom
{
  public required Guid UserId { get; set; }
  public virtual ApplicationUser? User { get; set; }
  public required Guid WarehouseId { get; set; }
  public virtual Warehouse? Warehouse { get; set; } 
}
