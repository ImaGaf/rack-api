namespace RackLogic.Core.Common.Entity;
public class FloorPlanFiles : BaseEntityCustom
{
  public required Guid FloorPlanId { get; set; }
  public virtual FloorPlan? FloorPlan { get; set; }
  public required string FileName { get; set; }
  public required string FileUrl { get; set; }
  public required string ContentType { get; set; }

}
