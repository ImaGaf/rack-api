namespace RackLogic.UseCases.Auths.RefeshToken;
public class RefreshTokenDTO
{
  public string? AccessToken { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime ExpiresIn { get; set; }
}
