using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.DefectLogs.Count;

public record CountDefectLogQuery(Guid WareHouseId) : IQuery<Result<CountDefectLogDto>>
{
}
