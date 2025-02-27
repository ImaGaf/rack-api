namespace RackLogic.Core.Common.Entity;
public class FloorPlanComments : BaseEntityCustom
{
  public required string UserName { get; set; }
  public required string Comment { get; set; }
  public required Guid FloorPlanItemId { get; set; }
  public FloorPlanItem? FloorPlanItem { get; set; }
}
