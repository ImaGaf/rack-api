using RackLogic.UseCases.FloorPlanItems.Inspection;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlanItems;

public interface IFloorPlanItemService
{
  Task<PagedDataDTO<InspectionFloorPlanItemDto>> GetListInspectionItem(InspectionFloorPlanItemQuery request);
}
