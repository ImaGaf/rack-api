using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.BayConfigurator;
using RackLogic.UseCases.BayConfigurator.List;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;
public class ListBayConfiguratorQueryService(AppDbContext _context) : IListBayConfiguratorQueryService
{
  public async Task<PagedDataDTO<BayConfigurationDTO>> ListAsync(ListBayConfiguratorQuery request)
  {
    if (request.AssetId != null)
    {
      return await FilterByAssetId(request);
    }

    var bayConfigurations = _context.BayConfigurations
          .Where(x => x.WarehouseId == request.WarehouseId)
          .Include(b => b.Components)
          .Include(b => b.BayConfigurationLevels)
          .Include(b => b.Manufacturer)
          .Include(b => b.SWLSignage)
          .OrderByDescending(i => i.CreatedDate);

    var totalRecord = await bayConfigurations.CountAsync();
    var result = new List<BayConfiguration>();
    var isAll = request.IsGetAll ?? false;
    if (!isAll)
    {
      result = await bayConfigurations
         .Skip((request.PageNumber - 1) * request.PageSize)
         .Take(request.PageSize)
         .ToListAsync();  
    }
    else
    {
      result = await bayConfigurations.ToListAsync();
    }

    return new PagedDataDTO<BayConfigurationDTO>()
    {
      TotalRecords = totalRecord,
      PageNumber = request.PageNumber,
      PageSize = !isAll ? request.PageSize : totalRecord,
      DataDTO = result.Select(x => x.Mapping()).ToList()
    };
  }

  private async Task<PagedDataDTO<BayConfigurationDTO>> FilterByAssetId(ListBayConfiguratorQuery request)
  {
    var bayConfigurations = await _context.FloorPlanItems
      .Where(x => x.FloorPlan!.WarehouseId == request.WarehouseId)
      .Where(x => x.FloorPlanId == request.AssetId || request.AssetId == null)
      .Where(x => x.Rack == request.RackNumber || request.RackNumber == 0 || request.RackNumber == null)
      .Include(x => x.BayConfiguration!)
           .ThenInclude(b => b.Components)
      .Include(x => x.BayConfiguration!)
          .ThenInclude(b => b.BayConfigurationLevels)
      .Include(x => x.BayConfiguration!)
          .ThenInclude(b => b.Manufacturer)
      .Include(x => x.BayConfiguration!)
          .ThenInclude(b => b.SWLSignage)
      .Select(x => x.BayConfiguration)
      .Distinct()
      .OrderByDescending(i => i!.CreatedDate)
      .ToListAsync();

    var totalRecord = bayConfigurations.Where(x => x != null).Count();

    var result = bayConfigurations
        .Where(x => x != null)
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize);

    return new PagedDataDTO<BayConfigurationDTO>()
    {
      TotalRecords = totalRecord,
      PageNumber = request.PageNumber,
      PageSize = totalRecord,
      DataDTO = result.Select(x => x!.Mapping()).ToList()
    };
  }
}
