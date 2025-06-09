using Ardalis.Result;
using RackLogic.UseCases.Inspections.End;
using RackLogic.UseCases.Inspections.ExportReport;
using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.UseCases.Inspections.Update;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections;

public interface IInspectionService
{
  Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  Task<PagedDataDTO<InspectionReportDto>> SearchReportAsync(SearchInspectionReportQuery request, Guid userId, CancellationToken cancellationToken);
  Task<InspectionDto?> UpdateAsync(UpdateInspectionCommand request, CancellationToken cancellationToken);
  Task<Result> EndAsync(EndInspectionCommand request, CancellationToken cancellationToken);
  Task<byte[]> ExportReportPdfAsync(ExportReportCommand command, CancellationToken cancellationToken);
  Task<string> GetReportHtmlAsync(Guid inspectionId, CancellationToken cancellationToken, bool hasDefectLog = false);
  Task<IEnumerable<InspectionDto>> GetAllAsync(Guid warehouseId, CancellationToken cancellationToken = default);
}
