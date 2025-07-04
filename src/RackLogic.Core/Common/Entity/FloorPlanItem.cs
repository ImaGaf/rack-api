namespace RackLogic.Core.Common.Entity;
public class FloorPlanItem : BaseEntityCustom
{
  public required int Rack { get; set; }
  public required int Bay { get; set; }
  public int? AssetOrdinal { get; set; }
  public required Guid FloorPlanId { get; set; }
  public Guid? BayConfigurationId { get; set; }
  public DateTime? DueDate { get; set; }
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  public string? Side { get; set; }
  public required double Rotation { get; set; }
  public BayConfiguration? BayConfiguration { get; set; }
  public virtual FloorPlan? FloorPlan { get; set; }
  public virtual ICollection<FloorPlanComments>? Comments { get; set; }
  public virtual ICollection<DefectLog>? DefectLogs { get; set; }
}
