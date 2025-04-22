using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlanItems.ListDefect;

public class GetListDefectLogByFloorPlanItemIdQueryHandler(
  IDefectLogService _defectLogService
  )
  : IQueryHandler<GetListDefectLogByFloorPlanItemIdQuery, Result<PagedDataDTO<DefectLogDto>>>
{
  public async Task<Result<PagedDataDTO<DefectLogDto>>> Handle(GetListDefectLogByFloorPlanItemIdQuery request, CancellationToken cancellationToken)
  {
    var defectLogs = await _defectLogService.GetListAsync(request, cancellationToken);

    return Result.Success(defectLogs);
  }
}
