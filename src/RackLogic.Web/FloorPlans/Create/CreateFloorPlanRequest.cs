using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
  public List<IFormFile> Files { get; set; } = new();
}
