namespace RackLogic.Core.Common.Entity;
public class FloorPlanItem : BaseEntityCustom
{
  public required int Rack { get; set; }
  public required int Bay { get; set; }
  public required Guid FloorPlanId { get; set; }
  public FloorPlan? FloorPlan { get; set; }
  public ICollection<FloorPlanComments>? Comments { get; set; }
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  public required double Rotation { get; set; }
}
