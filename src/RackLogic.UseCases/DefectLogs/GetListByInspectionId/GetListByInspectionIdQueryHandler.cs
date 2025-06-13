using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.DefectLogs.GetListByInspectionId;

public class GetListByInspectionIdQueryHandler : IQueryHandler<GetListByInspectionIdQuery, Result<IEnumerable<DefectLogDto>>>
{
  private readonly IDefectLogService _defectLogService;

  public GetListByInspectionIdQueryHandler(IDefectLogService defectLogService)
  {
    _defectLogService = defectLogService;
  }

  public async Task<Result<IEnumerable<DefectLogDto>>> Handle(GetListByInspectionIdQuery request, CancellationToken cancellationToken)
  {
    var defectLog = await _defectLogService.GetAllByInspectionIdAsync(request.InspectionId, cancellationToken);

    return Result.Success(defectLog);
  }
}
