using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.DefectLogs.Count;
using RackLogic.UseCases.DefectLogs.List;
using RackLogic.UseCases.FloorPlanItems.ListDefect;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;

public class DefectLogService(
  AppDbContext dbContext,
  IUser _user
) : IDefectLogService
{
  public async Task<CountDefectLogDto> CountAsync(CountDefectLogQuery request, CancellationToken cancellationToken = default)
  {
    var defectLogs = await dbContext.DefectLogs
        .Where(x => !x.IsCompleted)
        .Where(x => x.FloorPlanItem!.FloorPlan!.WarehouseId == request.WareHouseId)
        .ToListAsync(cancellationToken);

    var priorities = Enum.GetValues<DefectPriority>()
        .Select(priority => new PriorityDto
        {
          Type = priority,
          Count = defectLogs.Count(x => x.DefectPriority == priority)
        })
        .ToList();

    var statuses = Enum.GetValues<DefectStatus>()
        .Select(status => new StatusDto
        {
          Type = status,
          Count = defectLogs.Count(x => x.DefectStatus == status)
        }).ToList();

    return new CountDefectLogDto
    {
      Priorities = priorities,
      Statuses = statuses
    };
  }

  public async Task<DefectLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    var defectLog = await dbContext.DefectLogs
      .Include(d => d.Inspection)
      .Include(d => d.FloorPlanItem!.FloorPlan)
      .Include(d => d.DefectLogFiles)
      .Include(d => d.Component!.BayConfiguration!.Manufacturer)
      .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    if (defectLog is null)
      return defectLog;

    var histories = await dbContext.DefectLogHistories
      .Where(h => h.DefectLogId == id)
      .OrderByDescending(h => h.CreationTime)
      .Include(h => h.User)
      .ToListAsync(cancellationToken);

    defectLog!.DefectLogHistories = histories;

    return defectLog;
  }

  public async Task<PagedDataDTO<DefectLogDto>> GetListAsync(GetListDefectLogQuery request, CancellationToken cancellationToken = default)
  {
    var query = dbContext.DefectLogs
      .Include(x => x.Inspection)
      .Include(x => x.DefectLogFiles)
      .Include(x => x.Component!)
        .ThenInclude(c => c.BayConfiguration!)
          .ThenInclude(b => b.Manufacturer)
      .Include(x => x.FloorPlanItem!.FloorPlan)
      .Where(x => x.IsCompleted == request.IsCompleted)
      .Where(x => request.AssetId != null ? x.FloorPlanItem!.FloorPlanId == request.AssetId : true)
      .Where(x => request.Rack != null ? x.FloorPlanItem!.Rack == request.Rack : true)
      .Where(x => request.Bay != null ? x.FloorPlanItem!.Bay == request.Bay : true)
      .Where(x => request.DefectStatus != null ? x.DefectStatus == request.DefectStatus : true)
      .Where(x => request.DefectPriority != null ? x.DefectPriority == request.DefectPriority : true);

    if (request.IsCompleted)
    {
      var warehouses = await dbContext.UserWarehouseRoles.Where(x => x.UserId == _user.Id).Select(x => x.WarehouseId).ToListAsync();
      query = query.Where(x => warehouses.Contains(x.FloorPlanItem!.FloorPlan!.WarehouseId));
    }
    else
      query = query.Where(x => x.FloorPlanItem!.FloorPlan!.WarehouseId == request.WarehouseId);

    switch (request.SortBy)
    {
      case SortBy.Newest:
        query = query.OrderByDescending(x => x.CreationTime);
        break;
      case SortBy.Oldest:
        query = query.OrderBy(x => x.CreationTime);
        break;
      case SortBy.HighToLow:
        query = query.OrderBy(x => x.DefectPriority);
        break;
      case SortBy.LowToHigh:
        query = query.OrderByDescending(x => x.DefectPriority);
        break;
      default:
        query = query.OrderByDescending(x => x.CreationTime);
        break;
    }

    var result = await query.Skip((request.pageNumber - 1) * request.pageSize).Take(request.pageSize).ToListAsync();

    var totalItem = await query.CountAsync();

    return new PagedDataDTO<DefectLogDto>
    {
      TotalRecords = totalItem,
      PageNumber = request.pageNumber <= 0 ? PaginationDefaultValue.PageNumber : request.pageNumber,
      PageSize = request.pageSize <= 0 ? PaginationDefaultValue.PageSize : request.pageSize,
      DataDTO = result.Select(x => x.ToDtoWithHistory()).ToList()
    };
  }

  public async Task<PagedDataDTO<DefectLogDto>> GetListAsync(GetListDefectLogByFloorPlanItemIdQuery request, CancellationToken cancellationToken = default)
  {
    var query = dbContext.DefectLogs
      .Include(x => x.Inspection)
      .Include(x => x.DefectLogFiles)
      .Include(x => x.Component!).ThenInclude(c => c.BayConfiguration!).ThenInclude(b => b.Manufacturer)
      .Include(x => x.FloorPlanItem!.FloorPlan)
      .Where(x => !x.IsCompleted)
      .Where(x => x.FloorPlanItemId == request.floorPlanId && (x.InspectionId == request.inspectionId || request.inspectionId == Guid.Empty || request.inspectionId == null));

    var defectlogs = new List<DefectLog>();

    if (request.isGetAll)
      defectlogs = await query.ToListAsync(cancellationToken);
    else
      defectlogs = await query.Skip((request.pageNumber - 1) * request.pageSize).Take(request.pageSize).ToListAsync();

    var totalItem = await query.CountAsync();

    var result = new PagedDataDTO<DefectLogDto>
    {
      TotalRecords = totalItem,
      PageNumber = request.pageNumber <= 0 ? PaginationDefaultValue.PageNumber : request.pageNumber,
      PageSize = request.pageSize <= 0 ? PaginationDefaultValue.PageSize : request.pageSize,
      DataDTO = defectlogs.Select(x => x.ToDtoWithHistory()).ToList()
    };

    if (request.isGetAll)
    {
      result.PageNumber = 1;
      result.PageSize = totalItem;
    }

    return result;
  }

  public async Task<IEnumerable<DefectLogDto>> GetAllByInspectionIdAsync(Guid inspectionId, CancellationToken cancellationToken = default)
  {
    var defectLogs = await dbContext.DefectLogs
      .Include(x => x.Inspection)
      .Include(x => x.DefectLogFiles)
      .Include(x => x.Component!).ThenInclude(c => c.BayConfiguration!).ThenInclude(b => b.Manufacturer)
      .Include(x => x.FloorPlanItem!.FloorPlan)
      .Where(x => x.InspectionId == inspectionId)
      .ToListAsync();

    var result = defectLogs.Select(x => x.ToDtoWithHistory()).ToList();

    return result;
  }
}
