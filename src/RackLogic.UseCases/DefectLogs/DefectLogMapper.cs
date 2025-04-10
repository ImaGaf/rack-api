using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.DefectLogs.Create;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.DefectLogs;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  public static partial DefectLog ToEntity(this CreateDefectLogCommand command);
  public static partial DefectLogDto ToDto(this DefectLog entity);
}
