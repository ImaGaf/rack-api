namespace RackLogic.UseCases.Users;
public class InspectorProfileDTO
{
  public required Guid UserId { get; set; }
  public string? CompanyName { get; set; }
  public string? ABN { get; set; }
  public string? CompanyAddress { get; set; }
  public string? CompanyMainPhone { get; set; }
  public string? FileImagePath { get; set; }
  public string? InspectorFirstName { get; set; }
  public string? InspectorLastName { get; set; }
  public string? InspectorPhoneNumber { get; set; }
  public string? InspectorEmailAddress { get; set; }
}
