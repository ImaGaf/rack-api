using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class UserWarehouseRole : BaseEntityCustom
{
  public Guid UserId { get; set; }
  public required ApplicationUser User { get; set; }

  public Guid WareHoseId { get; set; }
  public required Warehouse Warehouse { get; set; }

  public Guid RoleId { get; set; }
  public required IdentityRole<Guid> Role { get; set; }
}
