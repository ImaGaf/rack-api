namespace RackLogic.UseCases.FloorPlans;
public class FloorPlanDTO()
{
  public Guid? FloorPlanId { get; set; }
  public required string Title { get; set; }
  public int Floor { get; set; }
  public string? Description { get; set; }
  public int TotalRack { get; set; }
  public int TotalBay { get; set; }
  public List<string> FileUrls { get; set; } = new();
  public List<FloorPlanItemResponse> Items { get; set; } = new();
  public DefaultSettingsResponse DefaultSettings { get; set; } = new();
}

public class FloorPlanItemResponse
{
  public Guid? Id { get; set; }
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
  public string? BayConfigurationId { get; set; }
  public DateTime? DueDate { get; set; }
  public int CountDefectLog { get; set; }
  public List<FloorPlanCommentResponse>? Comments { get; set; }
}

public class FloorPlanCommentResponse
{
  public Guid? Id { get; set; }
  public required string UserName { get; set; }
  public required string Comment { get; set; }
  public required string Date { get; set; }
}

public class DefaultSettingsResponse
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
