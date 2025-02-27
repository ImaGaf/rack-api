using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.FloorPlans;

namespace RackLogic.Infrastructure.Data.Queries;
public class FloorPlanService : IFloorPlanService
{
  private readonly AppDbContext _context;
  public FloorPlanService(AppDbContext context)
  {
    _context = context;
  }
  public async Task<List<FloorPlanDTO>> GetFloorPlansWithFilesByWarehouseIdAsync(Guid warehouseId)
  {
    //Get list FloorPlans by WarehouseId
    var floorPlans = await _context.FloorPlans
        .Include(f => f.FloorPlanFiles)
        .Include(f => f.FloorPlanItems)
        .ThenInclude(i => i.Comments)
        .Where(f => f.WarehouseId == warehouseId)
        .ToListAsync();

    return floorPlans.Select(FloorPlanUseCasesMapper.ToFloorPlanDTO).ToList();
  }
}
