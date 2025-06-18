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
  public required UpdateDefaultSetting DefaultSettings { get; set; }
  [Required]
  public required List<UpdateFloorPlanItemInformation> Items { get; set; } = new();
}
public class UpdateFloorPlanItemInformation
{
  public Guid? Id { get; set; }
  public UpdateFloorPlanItemInformation(Guid? id = null)
  {
    Id = id ?? Guid.NewGuid();
  }
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
  [Required]
  public string? BayConfigurationId { get; set; }
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
  [Required]
  public required string UserName { get; set; }
  [Required]
  public required string Comment { get; set; }
  [Required]
  public required DateTime Date { get; set; }
}
public class UpdateDefaultSetting
{
  public string? SingleBin { get; set; }
  public string? SingleBinDoubleDeep { get; set; }
  public string? PairSingleBin { get; set; }
  public string? PairSingleBinDoubleDeep { get; set; }
  public string? DoubleBin { get; set; }
  public string? DoubleBinDoubleDeep { get; set; }
  public string? PairDoubleBin { get; set; }
  public string? PairDoubleBinDoubleDeep { get; set; }
  public string? TripleBin { get; set; }
  public string? TripleBinDoubleDeep { get; set; }
  public string? PairTripleBin { get; set; }
  public string? PairTripleBinDoubleDeep { get; set; }
}
