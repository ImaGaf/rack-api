namespace RackLogic.Web.Users.UpdateInspectorProfile;

public class UpdateInspectorProfileRequest
{
  public string? CompanyName { get; set; }
  public string? ABN { get; set; }
  public string? CompanyAddress { get; set; }
  public string? CompanyMainPhone { get; set; }
  public IFormFile? FileImageInspector { get; set; }
  public string? InspectorFirstName { get; set; }
  public string? InspectorLastName { get; set; }
  public string? InspectorPhoneNumber { get; set; }
  public string? InspectorEmailAddress { get; set; }
}
