namespace RackLogic.UseCases.Users;
public class InspectorProfileDTO
{
  public required Guid UserId { get; set; }
  public required string CompanyName { get; set; }
  public required string ABN { get; set; }
  public required string CompanyAddress { get; set; }
  public required string CompanyMainPhone { get; set; }
  public required string FileImagePath { get; set; }
  public required string InspectorFirstName { get; set; }
  public required string InspectorLastName { get; set; }
  public required string InspectorPhoneNumber { get; set; }
  public required string InspectorEmailAddress { get; set; }
}
