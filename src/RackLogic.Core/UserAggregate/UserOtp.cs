using RackLogic.Core.Common.Entity;

namespace RackLogic.Core.UserAggregate;
public class UserOtp : BaseEntityCustom
{
  public Guid UserId { get; set; }
  public required string Otp { get; set; }
  public string? ResetToken { get; set; }
  public string? DeleteToken { get; set; }
  public virtual ApplicationUser? User { get; set; }
}
