using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.DefectLogs.Detail;

public record GetDetailDefectLogQuery(Guid defectLogId) : IQuery<Result<DefectLogDto>>
{
}
