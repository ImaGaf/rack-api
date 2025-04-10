using RackLogic.Core.Common.Entity;

namespace RackLogic.UseCases.DefectLogs;

public interface IDefectLogHistoryService
{
  Task<bool> CreateAsync(DefectLog defectLog, string? details, int type, CancellationToken cancellationToken = default);
}
