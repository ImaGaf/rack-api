using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.DefectLogs;

namespace RackLogic.Infrastructure.Data.Queries;

public class DefectLogService(
  AppDbContext dbContext
) : IDefectLogService
{
  public async Task<DefectLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    var defectLog = await dbContext.DefectLogs
      .Include(d => d.DefectLogFiles)
      .Include(d => d.Component)
      .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    var histories = await dbContext.DefectLogHistories
      .Where(h => h.DefectLogId == id)
      .OrderByDescending(h => h.CreationTime)
      .Take(4)
      .Include(h => h.User)
      .ToListAsync(cancellationToken);

    defectLog!.DefectLogHistories = histories;

    return defectLog;
  }
}
