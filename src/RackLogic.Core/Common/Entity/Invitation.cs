using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class Invitation : BaseEntityCustom
{
  public required Guid InviteById { get; set; }
  public ApplicationUser? User { get; set; }

  public required string Email { get; set; }

  public required Guid WarehouseId { get; set; }
  public Warehouse? Warehouse { get; set; }

  public required string Role { get; set; }
}
