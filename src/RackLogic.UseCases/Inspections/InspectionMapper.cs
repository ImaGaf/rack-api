using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.Inspections.Create;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Inspections;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  [MapProperty(nameof(Inspection.AuthorisePersonId), nameof(InspectionDto.AuthorisedPersonId))]
  public static partial InspectionDto ToDto(this Inspection entity);
  public static partial Inspection ToEntity(this CreateInspectionCommand command);
  public static partial Inspection ToEntity(this InspectionDto dto);
}
