namespace RackLogic.UseCases.FloorPlanItems.Inspection;

public class InspectionFloorPlanItemDto
{
  public Guid FloorPlanItemId { get; set; }
  public required string BayId { get; set; }
  public int? Rack { get; set; }
  public int? Bay { get; set; }
  public int? AssetOrdinal { get; set; }
  public string? Side { get; set; }
  public string? BayConfigId { get; set; }
  public DateTime? InspectionDueDate { get; set; }
  public int? Interval { get; set; }
}
