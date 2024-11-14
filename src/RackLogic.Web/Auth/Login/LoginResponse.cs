namespace RackLogic.Web.Auth.Login;

public class LoginResponse
{
  public string? AccessToken { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime ExpiresIn { get; set; }
}
