using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.DefectLogs.Count;

public class CountDefectLogQueryHandler(
  IDefectLogService _defectLogService)
  : IQueryHandler<CountDefectLogQuery, Result<CountDefectLogDto>>
{
  public async Task<Result<CountDefectLogDto>> Handle(CountDefectLogQuery request, CancellationToken cancellationToken)
  {
    var countDefectLog = await _defectLogService.CountAsync(request, cancellationToken);

    return Result.Success(countDefectLog);
  }
}
