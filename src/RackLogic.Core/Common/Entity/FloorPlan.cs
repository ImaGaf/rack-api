namespace RackLogic.Core.Common.Entity;
public class FloorPlan : BaseEntityCustom
{
  public required Guid WarehouseId { get; set; }
  public Warehouse? Warehouse { get; set; }
  public required string AssetName { get; set; }
  public required string Floor { get; set; }
  public required ICollection<FloorPlanFiles> FloorPlanFiles { get; set; }
}
