namespace RackLogic.Web.Users.ResetPassword;

public class ResetPasswordRequest
{
  public required string Email { get; set; }
  public required string NewPassword { get; set; }

  public required string ResetPasswordToken { get; set; }
}
