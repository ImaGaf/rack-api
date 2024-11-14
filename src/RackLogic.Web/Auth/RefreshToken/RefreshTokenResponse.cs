namespace RackLogic.Web.Auth.RefreshToken;

public class RefreshTokenResponse
{
  public string?  AccessToken { get; set; }
  public string?  RefreshToken { get; set; }
  public DateTime ExpiresIn { get; set; }
}
