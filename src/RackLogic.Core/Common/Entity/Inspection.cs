using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;

public class Inspection : BaseEntityCustom
{
  public Guid FloorPlanItemId { get; set; }
  public Guid AuthorisePersonId { get; set; }
  public DateTime InspectionDate { get; set; }
  public DateTime NextInspectionDate { get; set; }
  public string? BayId { get; set; }
  public string? ClientId { get; set; }
  public string? Authorise { get; set; }
  public bool IsConfirmAuthorization { get; set; }
  public virtual FloorPlanItem? FloorPlanItem { get; set; }
  public virtual ApplicationUser? AuthorisePerson { get; set; }
}
