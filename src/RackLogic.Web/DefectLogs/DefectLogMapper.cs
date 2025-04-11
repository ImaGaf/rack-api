using RackLogic.UseCases.DefectLogs.Update;
using RackLogic.Web.DefectLogs.Update;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.DefectLogs;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  public static partial UpdateDefectLogCommand ToCommand(this UpdateDefectLogRequest request);
}
