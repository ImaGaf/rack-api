using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;
public class InspectorProfile : BaseEntityCustom
{
  public required string Companyname { get; set; }
  public required string ABN { get; set; }
  public required string CompanyAddress { get; set; }
  public required string CompanyMainPhone { get; set; }
  public string? FileImageName { get; set; }
  public string? FileImageUrl { get; set; }
  public required string InspectorFirstName { get; set; }
  public required string InspectorLastName { get; set; }
  public required string InspectorEmailAddress { get; set; }
  public required string InspectorPhoneNumber { get; set; }
  public Guid UserId { get; set; }
  public ApplicationUser? User { get; set; }
}
