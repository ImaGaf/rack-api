using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class UserWarehouseRole : BaseEntityCustom
{
  public Guid UserId { get; set; }
  public ApplicationUser? User { get; set; }

  public Guid WarehouseId { get; set; }
  public Warehouse? Warehouse { get; set; }

  public Guid RoleId { get; set; }
  public IdentityRole<Guid>? Role { get; set; }
}
