namespace RackLogic.Web.Warehouses.InviteUser;

public class InviteUserRequest
{
  public Guid InvitedById { get; set; }
  public required string Email { get; set; }
  public Guid WarehouseId { get; set; }
  public required string Role { get; set; }
}
