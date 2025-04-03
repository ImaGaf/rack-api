using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlanItems.Inspection;

public record InspectionFloorPlanItemQuery(Guid WarehouseId, int pageNumber = 1, int pageSize = 6)
  : IQuery<Result<PagedDataDTO<InspectionFloorPlanItemDto>>>
{
}
