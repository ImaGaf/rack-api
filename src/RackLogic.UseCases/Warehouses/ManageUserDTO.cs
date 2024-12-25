

namespace RackLogic.UseCases.Warehouses;
public class ManageUserDTO
{
  public Guid UserID { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }
  public string? Role { get; set; }
}
