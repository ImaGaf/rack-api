using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.BayConfigurator;
using RackLogic.UseCases.BayConfigurator.List;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;
public class ListBayConfiguratorQueryService(AppDbContext _context) : IListBayConfiguratorQueryService
{
  public async Task<PagedDataDTO<BayConfigurationDTO>> ListAsync(Guid warehouseId, int pageNumber, int pageSize)
  {
    var bayConfigurations = _context.BayConfigurations
          .Where(x => x.WarahouseId == warehouseId)
          .Include(b => b.Components)
          .Include(b => b.BayConfigurationLevels)
          .Include(b => b.Manufacturer);
    var totalRecord = await bayConfigurations.CountAsync();
    var result = new List<BayConfiguration>();
    if (pageNumber > 0 && pageSize > 0)
    {
      result = await bayConfigurations
         .Skip((pageNumber - 1) * pageSize)
         .Take(pageSize)
         .ToListAsync();
    }
    else
    {
      result = await bayConfigurations.ToListAsync();
    }

    return new PagedDataDTO<BayConfigurationDTO>() 
    {
      TotalRecords = totalRecord,
      PageNumber = pageNumber,
      PageSize = pageSize,
      DataDTO = result.Select(x => x.Mapping()).ToList()
    };
  }
}
