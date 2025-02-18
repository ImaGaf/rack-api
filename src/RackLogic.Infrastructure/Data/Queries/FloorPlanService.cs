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

    return floorPlans.Select(f => new FloorPlanDTO
    {
      FloorPlanRequestId = f.Id,
      Title = f.AssetName,
      Floor = f.FloorNumber,
      Description = f.Description,
      TotalRack = f.TotalRack,
      TotalBay = f.TotalBay,
      FileUrls = f.FloorPlanFiles.Select(file => file.FileUrl).ToList(),
      Items = f.FloorPlanItems.Select(i => new FloorPlanItemResponse
      {
        Id = i.Id,
        Rack = i.Rack,
        Bay = i.Bay,
        Offset = i.Offset,
        Size = i.Size,
        Width = i.Width,
        Height = i.Height,
        ObjectType = i.ObjectType,
        Rotation = i.Rotation,
        Comments = i.Comments?.Select(c => new FloorPlanCommentResponse
        {
          Id = c.Id,
          UserName = c.UserName,
          Comment = c.Comment,
          Date = c.CreationTime.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList() ?? new List<FloorPlanCommentResponse>()
      }).ToList()
    }).ToList();
  }
}
