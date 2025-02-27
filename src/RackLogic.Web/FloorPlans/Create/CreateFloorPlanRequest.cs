using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.FloorPlans.Create;

public class CreateFloorPlanRequest
{
  [Required]
  public Guid WarehouseId { get; set; }
  [Required]
  public required List<FloorPlanRequest> FloorAsset { get; set; }
}

public class FloorPlanRequest
{
  public Guid? FloorPlanRequestId { get; set; } 
  [Required]
  public required string Title { get; set; }
  public int? Floor { get; set; }
  public string? Description { get; set; }
  public int? TotalRack { get; set; }
  public int? TotalBay { get; set; }
  [Required]
  public required List<FloorPlanItem> Items { get; set; }
  public List<IFormFile> Files { get; set; } = new();
}

public class FloorPlanItem
{
  public Guid? Id { get; set; }
  public int? Rack { get; set; }
  public int? Bay { get; set; }
  public List<Comment>? Comments { get; set; }
  [Required]
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  [Required]
  public required double Rotation { get; set; }
}

public class Comment
{
  public Guid? Id { get; set; }
  [Required]
  public required string userName { get; set; }
  [Required]
  public required string comment { get; set; }
}
