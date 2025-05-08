using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlanItems.ListDefect;

public class GetListDefectByFloorPlanItemIdRequest : PaginationRequest
{
  public required Guid FloorPlanItemId { get; set; }
  public Guid? InspectionId { get; set; }
}
