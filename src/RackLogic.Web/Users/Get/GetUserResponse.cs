namespace RackLogic.Web.Users.Get;

public class GetUserResponse
{
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }
  public string? PhoneNumber { get; set; }
  public string? RegistedRole { get; set; }
  public bool IsPaid { get; set; }
}
