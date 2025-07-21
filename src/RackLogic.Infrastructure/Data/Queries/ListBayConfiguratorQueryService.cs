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
    var query = _context.FloorPlanItems
      .Where(x => x.FloorPlan!.WarehouseId == request.WarehouseId)
      .Where(x => request.AssetId == null || x.FloorPlanId == request.AssetId)
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
      .OrderByDescending(i => i!.CreatedDate) as IQueryable<BayConfiguration>;

    query = query.Where(x => x!.BayConfigId == request.BayConfigId || string.IsNullOrEmpty(request.BayConfigId));

    var bayConfigurations = await query.ToListAsync();

    var totalRecord = bayConfigurations.Where(x => x != null).Count();

    var result = bayConfigurations
        .Where(x => x != null)
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize);

    return new PagedDataDTO<BayConfigurationDTO>()
    {
      TotalRecords = totalRecord,
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      DataDTO = result.Select(x => x!.Mapping()).ToList()
    };
  }
}
