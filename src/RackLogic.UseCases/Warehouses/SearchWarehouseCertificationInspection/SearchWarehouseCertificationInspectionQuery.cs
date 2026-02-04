using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.SearchWarehouseCertificationInspection;

public record SearchWarehouseCertificationInspectionQuery(string? Search, int PageNumber, int PageSize, string? OrderBy, bool OrderByDesc, string? ThenBy, bool ThenByDesc) : IQuery<Result<PagedDataDTO<WarehouseCertificationInspectionDto>>>;
