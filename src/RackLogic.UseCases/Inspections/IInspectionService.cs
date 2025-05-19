using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.UseCases.Warehouses;

using RackLogic.UseCases.Inspections.Update;
using RackLogic.UseCases.Inspections.ExportReport;
using RackLogic.UseCases.Inspections.End;
using Ardalis.Result;

namespace RackLogic.UseCases.Inspections;

public interface IInspectionService
{
  Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  Task<PagedDataDTO<InspectionReportDto>> SearchReportAsync(SearchInspectionReportQuery request, Guid userId, CancellationToken cancellationToken);
  Task<InspectionDto?> UpdateAsync(UpdateInspectionCommand request, CancellationToken cancellationToken);
  Task<Result> EndAsync(EndInspectionCommand request, CancellationToken cancellationToken);
  Task<byte[]> ExportReportPdfAsync(ExportReportCommand command, CancellationToken cancellationToken);
}
