namespace RackLogic.Web.FloorPlans.Get;

public class GetFloorPlanByIdResponse
{
  public Guid WarehouseId { get; set; }
  public string? AssetName { get; set; }
  public string? Floor { get; set; }
  public List<string> FileUrls { get; set; } = new();
}
