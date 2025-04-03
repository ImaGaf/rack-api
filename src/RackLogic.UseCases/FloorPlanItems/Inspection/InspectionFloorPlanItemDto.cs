namespace RackLogic.UseCases.FloorPlanItems.Inspection;

public class InspectionFloorPlanItemDto
{
  public Guid FloorPlanItemId { get; set; }
  public int Rack { get; set; }
  public int Bay { get; set; }
  public string? ConfigId { get; set; }
  public DateTime? InspectionDueDate { get; set; }
  public int? Interval { get; set; }
}
