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
  public required DefaultSettings? DefaultSettings { get; set; }
}

public class FloorPlanItem
{
  public Guid? Id { get; set; }
  public FloorPlanItem(Guid? id = null)
  {
    Id = id ?? Guid.NewGuid();
  }
  public required string BayId { get; set; }
  public int? Rack { get; set; }
  public int? Bay { get; set; }
  public List<Comment>? Comments { get; set; }
  [Required]
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  public int? AssetOrdinal { get; set; }
  public string? Side { get; set; }
  [Required]
  public required double Rotation { get; set; }
  public string? BayConfigId { get; set; }
  public bool? IsRequiredBayConfig { get; set; }
  public DateTime? DueDate { get; set; }
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
  public string? SingleBin { get; set; }
  public string? SingleBinDoubleDeep { get; set; }
  public string? PairSingleBin { get; set; }
  public string? PairSingleBinDoubleDeep { get; set; }
  public string? DoubleBin { get; set; }
  public string? DoubleBinDT { get; set; }
  public string? DoubleBinDoubleDeep { get; set; }
  public string? PairDoubleBin { get; set; }
  public string? PairDoubleBinDoubleDeep { get; set; }
  public string? TripleBin { get; set; }
  public string? TripleBinDT { get; set; }
  public string? TripleBinDoubleDeep { get; set; }
  public string? PairTripleBin { get; set; }
  public string? PairTripleBinDoubleDeep { get; set; }
}


