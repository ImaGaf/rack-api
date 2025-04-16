using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.DefectLogs.List;

public class GetListDefectLogQueryHandler(IDefectLogService _defectLogService)
  : IQueryHandler<GetListDefectLogQuery, Result<PagedDataDTO<DefectLogDto>>>
{
  public async Task<Result<PagedDataDTO<DefectLogDto>>> Handle(GetListDefectLogQuery request, CancellationToken cancellationToken)
  {
    var defectLogs = await _defectLogService.GetListAsync(request, cancellationToken);

    return Result.Success(defectLogs);
  }
}
