namespace RackLogic.Core.Common.Entity;
public class FloorPlan : BaseEntityCustom
{
  public required Guid WarehouseId { get; set; }
  public required string AssetName { get; set; }
  public required int FloorNumber { get; set; }
  public string? Description { get; set; }
  public required int TotalRack { get; set; }
  public required int TotalBay { get; set; }
  public virtual Warehouse? Warehouse { get; set; }
  public required ICollection<FloorPlanFiles> FloorPlanFiles { get; set; }
  public required ICollection<FloorPlanItem> FloorPlanItems { get; set; }
  public required ICollection<DefaultSettings> DefaultSettings { get; set; }
}
