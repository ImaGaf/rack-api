namespace RackLogic.Web.FloorPlanItems.ListDefect;

public class GetListDefectByFloorPlanItemIdRequest
{
  public required Guid FloorPlanItemId { get; set; }
  public int PageNumber { get; set; } = 1;
  public int PageSize { get; set; } = 6;
}
