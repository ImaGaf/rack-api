using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.DefectLogs.GetListByInspectionId;

public record GetListByInspectionIdQuery(Guid InspectionId) : IQuery<Result<IEnumerable<DefectLogDto>>>
{
}
