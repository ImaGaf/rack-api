using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Auth.RefreshToken;

public class RefreshTokenRequest
{
  [Required]
  public string? AccessToken { get; set; }
  [Required]
  public string? RefreshToken { get; set; }
}
