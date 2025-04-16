using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.SearchWarehouse;

public record SearchWarehouseQuery(string? Search, int PageNumber, int PageSize) : IQuery<Result<PagedDataDTO<WarehouseDto>>>;
