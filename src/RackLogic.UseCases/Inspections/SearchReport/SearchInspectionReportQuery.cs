using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections.SearchReport;

public record SearchInspectionReportQuery(string? Search, int PageNumber, int PageSize) : IQuery<Result<PagedDataDTO<InspectionReportDto>>>;
