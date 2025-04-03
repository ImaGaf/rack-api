namespace RackLogic.Web.FloorPlanItems.ListInspectionFloorPlanItem;

public class InspectionFloorPlanItemRequest
{
  public required Guid WarehouseId { get; set; }
  public int pageNumber { get; set; }
  public int pageSize { get; set; }
}
