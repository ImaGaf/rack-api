namespace RackLogic.Web.FloorPlans.List;

public record GetListFloorPlanRequest
{
  public Guid WarehouseId { get; set; }
}
