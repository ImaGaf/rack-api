using RackLogic.UseCases.FloorPlanItems.GetDetail;
using RackLogic.UseCases.FloorPlanItems.Inspection;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlanItems;

public interface IFloorPlanItemService
{
  Task<PagedDataDTO<InspectionFloorPlanItemDto>> GetListInspectionItemAsync(InspectionFloorPlanItemQuery request, CancellationToken cancellationToken = default);
  Task<FloorPlanItemDto?> GetDetailAsync(GetDetailFloorPlanItemQuery query, CancellationToken cancellationToken = default);
  Task<bool> UpdateInspectionDueDate(Guid floorPlanItemId);
}
