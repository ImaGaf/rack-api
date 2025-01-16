using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Users.Delete;

public class DeleteRequest
{
  [Required]
  public Guid UserId { get; set; }

  [Required]
  public required string DeleteAccountToken { get; set; }
}
