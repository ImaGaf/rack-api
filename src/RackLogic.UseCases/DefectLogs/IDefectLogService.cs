using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.DefectLogs.Count;
using RackLogic.UseCases.DefectLogs.List;
using RackLogic.UseCases.FloorPlanItems.ListDefect;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.DefectLogs;

public interface IDefectLogService
{
  Task<CountDefectLogDto> CountAsync(CountDefectLogQuery request, CancellationToken cancellationToken);
  Task<DefectLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  Task<PagedDataDTO<DefectLogDto>> GetListAsync(GetListDefectLogQuery request, CancellationToken cancellationToken = default);
  Task<PagedDataDTO<DefectLogDto>> GetListAsync(GetListDefectLogByFloorPlanItemIdQuery request, CancellationToken cancellationToken = default);
}
