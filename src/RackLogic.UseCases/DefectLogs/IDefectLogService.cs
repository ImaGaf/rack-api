using RackLogic.Core.Common.Entity;

namespace RackLogic.UseCases.DefectLogs;

public interface IDefectLogService
{
  Task<DefectLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
