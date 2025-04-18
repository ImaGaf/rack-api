using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.DefectLogs.List;

public record GetListDefectLogQuery(
    Guid WarehouseId,
    Guid? AssetId,
    int? Rack,
    int? Bay,
    SortBy? SortBy,
    int pageNumber = 1,
    int pageSize = 6
) : IQuery<Result<PagedDataDTO<DefectLogDto>>>;
