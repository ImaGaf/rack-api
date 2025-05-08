using RackLogic.UseCases.Inspections.Get;
using RackLogic.Web.Inspections.Get;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Inspections;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  public static partial GetInspectionByIdQuery ToQuery(this GetInspectionByIdRequest entity);
}
