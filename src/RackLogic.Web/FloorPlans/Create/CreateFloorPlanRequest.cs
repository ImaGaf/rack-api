using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.FloorPlans.Create;

public class RequestCreateFloorPlan
{
  [FromForm]
  public required string RequestFloorPlanData { get; set; }

  [FromForm]
  public List<IFormFile> Files { get; set; } = new();
}

public class CreateFloorAssetRequest
{
  [Required]
  public Guid WarehouseId { get; set; }
  [Required]
  public required List<FloorPlanRequest> FloorAssets { get; set; }
}

public class FloorPlanRequest
{
  public Guid? FloorPlanId { get; set; }

  public FloorPlanRequest(Guid? floorPlanId = null)
  {
    FloorPlanId = floorPlanId ?? Guid.NewGuid();
  }
  [Required]
  public required string Title { get; set; }
  [Required]
  public required int Floor { get; set; }
  public string? Description { get; set; }
  [Required]
  public required int TotalRack { get; set; }
  [Required]
  public required int TotalBay { get; set; }
  [Required]
  public required List<FloorPlanItem> Items { get; set; }
  public List<IFormFile>? Files { get; set; } = new();
  [Required]
  public required DefaultSettings DefaultSettings { get; set; }
}

public class FloorPlanItem
{
  public Guid? Id { get; set; }
  public FloorPlanItem(Guid? id = null)
  {
    Id = id ?? Guid.NewGuid();
  }
  [Required]
  public required int Rack { get; set; }
  [Required]
  public required int Bay { get; set; }
  public List<Comment>? Comments { get; set; }
  [Required]
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  [Required]
  public required double Rotation { get; set; }
  public required Guid BayConfigurationId { get; set; }
}

public class Comment
{
  public Guid? Id { get; set; }
  public Comment(Guid? id = null)
  {
    Id = id ?? Guid.NewGuid(); 
  }
  [Required]
  public required string userName { get; set; }
  [Required]
  public required string comment { get; set; }
}

public class DefaultSettings
{
  public Guid SingleBin { get; set; }
  public Guid PairSingleBin { get; set; }
  public Guid DoubleBin { get; set; }
  public Guid PairDoubleBin { get; set; }
  public Guid TripleBin { get; set; }
  public Guid PairTripleBin { get; set; }
  public Guid QuadBin { get; set; }
  public Guid PairQuadBin { get; set; }
}


