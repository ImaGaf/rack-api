using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.FloorPlans.Create;

public class CreateFloorPlanRequest
{
  [Required]
  public Guid WarehouseId { get; set; }
  [Required]
  public required List<FloorPlanRequest> FloorPlans { get; set; }
}

public class FloorPlanRequest
{
  [Required]
  public required string AssetName { get; set; }
  [Required]
  public required string Floor { get; set; }
}
