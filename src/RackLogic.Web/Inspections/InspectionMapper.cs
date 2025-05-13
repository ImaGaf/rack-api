using RackLogic.UseCases.Inspections.Get;
using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.Web.Inspections.Get;
using RackLogic.Web.Inspections.SearchReport;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Inspections;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  public static partial GetInspectionByIdQuery ToQuery(this GetInspectionByIdRequest entity);
  public static partial SearchInspectionReportQuery ToQuery(this SearchInspectionReportRequest entity);
}
