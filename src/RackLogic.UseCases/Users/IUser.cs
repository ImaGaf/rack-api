namespace RackLogic.UseCases.Users;

public interface IUser
{
  public Guid? Id { get; }
  public bool? IsAuthenticated { get; }
  public string? UserName { get; }
  public string? WarehouseRole { get; }
  public string? Role { get; }
  public IEnumerable<string>? Permissions { get; }
}
