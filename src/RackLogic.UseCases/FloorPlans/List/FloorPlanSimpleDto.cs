namespace RackLogic.UseCases.FloorPlans.List;

public class FloorPlanSimpleDto
{
  public required Guid AssetId { get; set; }
  public required string AssetName { get; set; }
  public required int FloorNumber { get; set; }
  public required int TotalRack { get; set; }
  public required int TotalBay { get; set; }
}
