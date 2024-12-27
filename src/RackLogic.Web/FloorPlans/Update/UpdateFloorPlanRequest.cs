using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.FloorPlans.Update;

public class UpdateFloorPlanRequest
{
  [Required]
  public Guid WarehouseId { get; set; }
  [Required]
  public required List<UpdateFloorPlanInformation> FloorPlans { get; set; }
}

public class UpdateFloorPlanInformation
{
  [Required]
  public Guid FloorPlanId { get; set; }
  [Required]
  public required string AssetName { get; set; }
  [Required]
  public required string Floor { get; set; }
}
