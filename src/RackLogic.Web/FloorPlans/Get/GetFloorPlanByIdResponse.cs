namespace RackLogic.Web.FloorPlans.Get;

public class GetFloorPlanByIdResponse
{
  public Guid? FloorPlanRequestId { get; set; }
  public required string Title { get; set; }
  public int Floor { get; set; }
  public string? Description { get; set; }
  public int TotalRack { get; set; }
  public int TotalBay { get; set; }
  public List<string> FileUrls { get; set; } = new();
  public List<FloorPlanItemResponse> Items { get; set; } = new();
}
public class FloorPlanItemResponse
{
  public Guid? Id { get; set; }
  public required int Rack { get; set; }
  public required int Bay { get; set; }
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  public required double Rotation { get; set; }
  public List<FloorPlanCommentResponse>? Comments { get; set; }
}

public class FloorPlanCommentResponse
{
  public Guid? Id { get; set; }
  public required string UserName { get; set; }
  public required string Comment { get; set; }
  public required string Date { get; set; }
}
