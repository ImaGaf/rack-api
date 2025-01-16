using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Users.ConfirmPassword;

public class ConfirmPasswordRequest
{
  [Required]
  public required string Email { get; set; }

  [Required]
  public required string OldPassword { get; set; }
}
