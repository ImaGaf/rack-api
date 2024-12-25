namespace RackLogic.Web.Warehouses.EditRoleUser;

public class EditRoleUserRequest
{
  public Guid UserId { get; set; }
  public Guid WarehouseId { get; set; }
  public required string Role { get; set; }
}
