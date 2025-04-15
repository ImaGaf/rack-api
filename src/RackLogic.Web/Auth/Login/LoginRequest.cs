using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Auth.Login;

public class LoginRequest
{
  [Required]
  public string? Email { get; set; }
  [Required]
  public string? Password { get; set; }
  public bool isRememberMe { get; set; }
}
