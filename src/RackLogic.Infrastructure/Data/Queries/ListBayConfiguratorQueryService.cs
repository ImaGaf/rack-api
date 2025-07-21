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
    var bayConfigurations = _context.BayConfigurations
          .Where(x => x.WarehouseId == request.WarehouseId)
          .Where(x => x.BayConfigId == request.BayConfigId || string.IsNullOrEmpty(request.BayConfigId))
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
}
