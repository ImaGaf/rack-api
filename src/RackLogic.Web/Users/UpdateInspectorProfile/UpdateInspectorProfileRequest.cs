using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.Users.UpdateInspectorProfile;

public class UpdateInspectorProfileRequest
{
  [Required]
  public required string CompanyName { get; set; }
  [Required]
  public required string ABN { get; set; }
  [Required]
  public required string CompanyAddress { get; set; }
  [Required]
  public required string CompanyMainPhone { get; set; }
  [Required]
  public required IFormFile FileImageInspector { get; set; }
  [Required]
  public required string InspectorFirstName { get; set; }
  [Required]
  public required string InspectorLastName { get; set; }
  [Required]
  public required string InspectorPhoneNumber { get; set; }
  [Required]
  public required string InspectorEmailAddress { get; set; }
}
