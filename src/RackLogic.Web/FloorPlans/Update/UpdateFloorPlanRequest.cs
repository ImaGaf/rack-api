using System.ComponentModel.DataAnnotations;

namespace RackLogic.Web.FloorPlans.Update;

public class UpdateFloorPlanRequest
{
  [Required]
  public Guid WarehouseId { get; set; }
  [Required]
  public required List<UpdateFloorPlanInformation> FloorAsset { get; set; }
}

public class UpdateFloorPlanInformation
{
  [Required]
  public Guid FloorPlanId { get; set; }
  public List<string>? FileUrlsToDelete { get; set; } = new();
  public List<IFormFile> NewFiles { get; set; } = new();
  public List<IFormFile> FilesToUpdate { get; set; } = new();
  [Required]
  public required string Title { get; set; }
  [Required]
  public required int Floor { get; set; }
  public required string? Description { get; set; }
  [Required]
  public required int TotalRack { get; set; }
  [Required]
  public required int TotalBay { get; set; }
  [Required]
  public required List<UpdateFloorPlanItemInformation> Items { get; set; } = new();
}
public class UpdateFloorPlanItemInformation
{
  [Required]
  public required Guid Id { get; set; }
  [Required]
  public required int Rack { get; set; }
  [Required]
  public required int Bay { get; set; }
  [Required]
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  [Required]
  public required double Rotation { get; set; }
  public required List<UpdateFloorPlanComment>? Comments { get; set; }
}
public class UpdateFloorPlanComment
{
  [Required]
  public Guid Id { get; set; }
  [Required]
  public required string UserName { get; set; }
  [Required]
  public required string Comment { get; set; }
  [Required]
  public required string Date { get; set; }
}
