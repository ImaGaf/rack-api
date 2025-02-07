using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Users.Delete;

public class DeleteRequest
{
  [Required]
  public required string DeleteAccountToken { get; set; }
}
