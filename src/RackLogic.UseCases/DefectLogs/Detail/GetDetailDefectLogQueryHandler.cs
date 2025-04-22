using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.DefectLogs.Detail;

public class GetDetailDefectLogQueryHandler(
  IDefectLogService _defectLogService)
  : IQueryHandler<GetDetailDefectLogQuery, Result<DefectLogDto>>
{
  public async Task<Result<DefectLogDto>> Handle(GetDetailDefectLogQuery request, CancellationToken cancellationToken)
  {
    var defectLog = await _defectLogService.GetByIdAsync(request.defectLogId, cancellationToken);

    return defectLog is null
      ? Result<DefectLogDto>.NotFound()
      : Result<DefectLogDto>.Success(defectLog.ToDtoWithHistory());
  }
}
