using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RackLogic.Core.UserAggregate;
public class ApplicationUser : IdentityUser<Guid>
{
  [Required]
  public string? FirstName { get; set; }
  [Required]
  public string? LastName { get; set; }
  [Required]
  public string? Postcode { get; set; }
  public bool OptOutRackLogicMarketing { get; set; }
  public int OtpSendCount { get; set; }
  public DateTime? LockedUntil { get; set; }
}

