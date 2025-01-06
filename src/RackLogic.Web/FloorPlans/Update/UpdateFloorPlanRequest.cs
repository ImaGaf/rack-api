using System.ComponentModel.DataAnnotations;
using RackLogic.UseCases.FloorPlans;

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
  public List<string>? FileUrlsToDelete { get; set; } = new();
  public List<IFormFile> NewFiles { get; set; } = new();
  public List<IFormFile> FilesToUpdate { get; set; } = new();

  [Required]
  public required string AssetName { get; set; }
  [Required]
  public required string Floor { get; set; }
}

