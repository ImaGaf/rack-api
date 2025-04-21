using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Users.Register;

public class RegisterUserRequest
{
  [Required]
  public string? FirstName { get; set; }
  [Required]
  public string? LastName { get; set; }
  [Required]
  public string? EmailAddress { get; set; }
  [Required]
  [MaxLength(10)]
  public string? PhoneNumber { get; set; }
  [Required]
  [MaxLength(4)]
  public string? Postcode { get; set; }
  [Required]
  public string? Password { get; set; }

  public bool OptOutRackLogicMarketing { get; set; }
  [Required]
  public string? Role { get; set; }

  public Guid? WarehouseId { get; set; }
}
