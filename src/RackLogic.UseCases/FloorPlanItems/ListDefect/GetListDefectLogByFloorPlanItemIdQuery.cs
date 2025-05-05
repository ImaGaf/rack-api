using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlanItems.ListDefect;

public record GetListDefectLogByFloorPlanItemIdQuery(
  Guid floorPlanId,
  Guid inspectionId,
  int pageNumber = 1,
  int pageSize = 6) : IQuery<Result<PagedDataDTO<DefectLogDto>>>
{
}
