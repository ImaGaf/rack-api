using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class UserWarehouseRole : BaseEntityCustom
{
  public Guid UserId { get; set; }
  public Guid RoleId { get; set; }
  public Guid WarehouseId { get; set; }
  public virtual ApplicationUser? User { get; set; }
  public virtual Warehouse? Warehouse { get; set; }
  public virtual IdentityRole<Guid>? Role { get; set; }
}
