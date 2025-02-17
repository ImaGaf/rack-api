namespace RackLogic.Core.Common.Entity;
public class FloorPlan : BaseEntityCustom
{
  public required Guid WarehouseId { get; set; }
  public Warehouse? Warehouse { get; set; }
  public required string AssetName { get; set; }
  public required int FloorNumber {  get; set; }
  public string? Description  { get; set; }
  public required int totalRack { get; set; }
  public required int totalBay { get; set; }
  public required ICollection<FloorPlanFiles> FloorPlanFiles { get; set; }
  public required ICollection<FloorPlanItem> FloorPlanItems { get; set; } 
}
