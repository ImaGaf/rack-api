using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.DefectLogs;

namespace RackLogic.Infrastructure.Data.Queries;

public class DefectLogHistoryService(
  AppDbContext dbContext,
  IRepositoryWithExpression<DefectLogHistory> defectLogHistoryRepository
) : IDefectLogHistoryService
{
  public async Task<bool> CreateAsync(DefectLog defectLog, string? details, int type, CancellationToken cancellationToken)
  {
    var entity = new DefectLogHistory
    {
      Id = Guid.NewGuid(),
      DefectLogId = defectLog.Id,
      UserId = defectLog.CreatorId!.Value,
      Note = details,
      Type = (DefectHistoryType)type
    };

    await defectLogHistoryRepository.AddToContextAsync(entity, cancellationToken: cancellationToken);

    await dbContext.SaveChangesAsync(cancellationToken);

    return true;
  }
}
