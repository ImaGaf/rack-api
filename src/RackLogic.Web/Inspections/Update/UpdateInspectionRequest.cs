namespace RackLogic.Web.Inspections.Update;

public record UpdateInspectionRequest
{
  public Guid InspectionId { get; init; }
  public Guid FloorPlanItemId { get; init; }
  public bool IsConfirmAuthorization { get; init; }
}
