using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections;

public interface IInspectionService
{
  Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  Task<PagedDataDTO<InspectionReportDto>> SearchReportAsync(SearchInspectionReportQuery request, Guid userId, CancellationToken cancellationToken);
}
