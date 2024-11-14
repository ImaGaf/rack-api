namespace RackLogic.UseCases.Auths.Login;
public class LoginDTO 
{ 
  public string? AccessToken { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime ExpiresIn { get; set; }
};
