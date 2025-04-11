using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.DefectLogs.Create;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.DefectLogs;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  public static partial DefectLog ToEntity(this CreateDefectLogCommand command);

  [MapperIgnoreSource(nameof(DefectLog.DefectLogHistories))]
  public static partial DefectLogDto ToDto(this DefectLog entity);

  public static DefectLogDto ToDtoWithHistory(this DefectLog entity, bool includeHistories = true)
  {
    var dto = entity.ToDto();

    if (includeHistories && entity.DefectLogHistories != null)
    {
      dto.DefectLogHistories = entity.DefectLogHistories
        .Select(history => new DefectLogHistoryDto
        {
          DefectLogId = history.DefectLogId,
          UserId = history.UserId,
          Note = history.Note,
          CreationTime = history.CreationTime,
          Type = history.Type,
          FirstName = history.User?.FirstName,
          LastName = history.User?.LastName
        }).ToList();
    }

    return dto;
  }
}
