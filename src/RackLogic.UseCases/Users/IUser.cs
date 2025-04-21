namespace RackLogic.UseCases.Users;

public interface IUser
{
  public Guid? Id { get; }
  public bool? IsAuthenticated { get; }
  public string? UserName { get; }
}
