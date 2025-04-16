using RackLogic.Core.Common.Enums;

namespace RackLogic.Web.DefectLogs.List;

public record GetListDefectLogRequest(
    Guid WarehouseId,
    Guid? AssetId,
    int? Rack,
    int? Bay,
    SortBy SortBy,
    int pageNumber = 1,
    int pageSize = 6
);
