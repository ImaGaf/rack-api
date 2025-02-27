using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Entity;

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
  public string? RegisteredRole { get; set; }
  public bool IsPaid { get; set; }
  public Guid? ParentId { get; set; }
  public DateTime? DeleteDate { get; set; }
  public ICollection<Warehouse> OwnedWarehouses { get; set; } = new List<Warehouse>();
  public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
  public ICollection<PurchaseRecord> Purchases { get; set; } = new List<PurchaseRecord>();
  public ICollection<UserOtp> UserOtps { get; set; } = new List<UserOtp>();
}

