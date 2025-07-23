using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.FloorPlans.Update;

public class RequestUpdateFloorPlan
{
  [FromForm]
  public required string RequestFloorPlanData { get; set; }

  [FromForm]
  public List<IFormFile> Files { get; set; } = new();
}

public class UpdateFloorPlanRequest
{
  [Required]
  public Guid WarehouseId { get; set; }
  [Required]
  public required List<UpdateFloorPlanInformation> FloorAssets { get; set; }
}

public class UpdateFloorPlanInformation
{
  public Guid? FloorPlanId { get; set; }
  public UpdateFloorPlanInformation(Guid? floorPlanId = null)
  {
    FloorPlanId = floorPlanId ?? Guid.NewGuid();
  }
  public List<string>? FileUrlsToDelete { get; set; } = new();
  public List<IFormFile>? Files { get; set; } = new();
  public required string Title { get; set; }
  public required int Floor { get; set; }
  public required string? Description { get; set; }
  public required int TotalRack { get; set; }
  public required int TotalBay { get; set; }
  public required UpdateDefaultSetting? DefaultSettings { get; set; }
  public required List<UpdateFloorPlanItemInformation> Items { get; set; } = new();
}
public class UpdateFloorPlanItemInformation
{
  public Guid? Id { get; set; }
  public UpdateFloorPlanItemInformation(Guid? id = null)
  {
    Id = id ?? Guid.NewGuid();
  }
  public required string BayId { get; set; }
  public int? Rack { get; set; }
  public int? Bay { get; set; }
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  public int? AssetOrdinal { get; set; }
  public string? Side { get; set; }
  public required double Rotation { get; set; }
  public string? BayConfigId { get; set; }
  public bool? IsRequiredBayConfig { get; set; }
  public DateTime? DueDate { get; set; }
  public required List<UpdateFloorPlanComment>? Comments { get; set; }
}
public class UpdateFloorPlanComment
{
  public Guid? Id { get; set; }
  public UpdateFloorPlanComment(Guid? id = null)
  {
    Id = id ?? Guid.NewGuid();
  }
  public required string UserName { get; set; }
  public required string Comment { get; set; }
  public required DateTime Date { get; set; }
}
public class UpdateDefaultSetting
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
