using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.Infrastructure.Data.Queries;
public class FloorPlanService : IFloorPlanService
{
  private readonly AppDbContext _context;
  private readonly IFileService _fileService;
  private readonly IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository;
  private readonly RoleManager<IdentityRole<Guid>> _roleManager;
  public FloorPlanService(AppDbContext context, 
                          IFileService fileService, 
                          IRepositoryWithExpression<UserWarehouseRole> userWarehouseRoleRepository,
                          RoleManager<IdentityRole<Guid>> roleManager)
  {
    _context = context;
    _fileService = fileService;
    _userWarehouseRoleRepository = userWarehouseRoleRepository;
    _roleManager = roleManager;
  }

  public async Task<List<Guid>> CheckAndGetErrorsForFloorPlanCommentsAsync(List<FloorPlanComments> floorPlanComments, List<Guid> errorFloorPlanItemIds)
  {
    var floorPlanCommentIds = new HashSet<Guid>(floorPlanComments.Select(f => f.Id));
    var existingFloorPlanComments = await _context.FloorPlanComments.Where(f => floorPlanCommentIds.Contains(f.Id)).ToListAsync();
    var existingFloorPlanCommentIds = new HashSet<Guid>(existingFloorPlanComments.Select(e => e.Id));

    var errorFloorPlanItemIdsSet = new HashSet<Guid>(errorFloorPlanItemIds);

    var errorFloorPlanCommentIds = floorPlanComments
        .Where(floorPlanComment =>
            errorFloorPlanItemIdsSet.Contains(floorPlanComment.FloorPlanItemId) ||
            existingFloorPlanCommentIds.Contains(floorPlanComment.Id))
        .Select(floorPlanComment => floorPlanComment.Id)
        .ToList();

    return errorFloorPlanCommentIds;
  }

  public async Task<List<Guid>> CheckAndGetErrorsForFloorPlanItemsAsync(List<FloorPlanItem> floorPlanItems, List<Guid> errorFloorPlanIds)
  {
    var floorPlanItemIds = new HashSet<Guid>(floorPlanItems.Select(f => f.Id));
    var existingFloorPlanItems = await _context.FloorPlanItems.Where(f => floorPlanItemIds.Contains(f.Id)).ToListAsync();
    var existingFloorPlanItemIds = new HashSet<Guid>(existingFloorPlanItems.Select(e => e.Id));

    var errorFloorPlanIdsSet = new HashSet<Guid>(errorFloorPlanIds);

    var errorFloorPlanItemIds = floorPlanItems
        .Where(floorPlanItem =>
            errorFloorPlanIdsSet.Contains(floorPlanItem.FloorPlanId) ||
            existingFloorPlanItemIds.Contains(floorPlanItem.Id))
        .Select(floorPlanItem => floorPlanItem.Id)
        .ToList();

    return errorFloorPlanItemIds;
  }

  public async Task<List<Guid>> CheckAndGetErrorsForFloorPlansAsync(List<FloorPlan> floorPlans)
  {
    var floorPlanIds = floorPlans.Select(f => f.Id).ToList();
    var existingFloorPlans = await _context.FloorPlans.Where(f => floorPlanIds.Contains(f.Id)).ToListAsync();
    return existingFloorPlans.Select(f => f.Id).ToList();
  }

  public (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanData(CreateFloorPlanCommand request, Guid userCreatedId)
  {
    var floorPlans = request.FloorPlans
        .Select(f => f.ToFloorPlan(request.WarehouseId, userCreatedId))
        .ToList();

    var floorPlanItems = request.FloorPlans
        .SelectMany(f => f.Items, (f, item) => item.ToFloorPlanItem(f.FloorPlanRequestId))
        .ToList();

    var floorPlanComments = request.FloorPlans
        .SelectMany(f => f.Items)
        .Where(item => item.Comments != null)
        .SelectMany(item => item.Comments ?? new List<FloorPlanCommentCommand>(), (item, comment) => comment.ToFloorPlanComment(item.Id))
        .ToList();

    return (floorPlans, floorPlanItems, floorPlanComments);
  }

  public (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanDataForUpdate( List<UpdateFloorPlanInformationCommand> nonMatchingPlans, Guid warehouseId, Guid userCreatedId)
  {
    var floorPlans = nonMatchingPlans
        .Select(f => f.ToFloorPlan(warehouseId, userCreatedId))
        .ToList();

    var floorPlanItems = nonMatchingPlans
        .SelectMany(f => f.Items, (f, item) => item.ToFloorPlanItem(f.FloorPlanId)) 
        .ToList();

    var floorPlanComments = nonMatchingPlans
        .SelectMany(f => f.Items)
        .Where(item => item.Comments != null)
        .SelectMany(item => item.Comments ?? new List<UpdateFloorPlanCommentCommand>(),
                    (item, comment) => comment.ToFloorPlanComment(item.Id))
        .ToList();

    return (floorPlans, floorPlanItems, floorPlanComments);
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

  public async Task<Result<bool>> SaveFloorPlanDataAsync(List<FloorPlan> floorPlans, List<FloorPlanItem> floorPlanItems, List<FloorPlanComments> floorPlanComments, List<FloorPlanCommand> floorPlanRequests)
  {
    // Check errors for each object
    var errorFloorPlanIds = await CheckAndGetErrorsForFloorPlansAsync(floorPlans);
    var errorFloorPlanItemIds = await CheckAndGetErrorsForFloorPlanItemsAsync(floorPlanItems, errorFloorPlanIds);
    var errorFloorPlanCommentIds = await CheckAndGetErrorsForFloorPlanCommentsAsync(floorPlanComments, errorFloorPlanItemIds);

    // Save valid floorPlans to DB first
    var validFloorPlans = floorPlans.Where(f => !errorFloorPlanIds.Contains(f.Id)).ToList();
    await _context.FloorPlans.AddRangeAsync(validFloorPlans);

    // Save valid floorPlanItems to DB first
    var validFloorPlanItems = floorPlanItems.Where(i => !errorFloorPlanItemIds.Contains(i.Id)).ToList();
    await _context.FloorPlanItems.AddRangeAsync(validFloorPlanItems);

    // Save valid floorPlanComments to DB first
    var validFloorPlanComments = floorPlanComments.Where(c => !errorFloorPlanCommentIds.Contains(c.Id)).ToList();
    await _context.FloorPlanComments.AddRangeAsync(validFloorPlanComments);

    // Save only valid files
    var validFloorPlanRequests = floorPlanRequests
    .Where(f => validFloorPlans.Any(vf => vf.Id == f.FloorPlanRequestId))
    .ToList();

    await SaveFloorPlanFilesAsync(validFloorPlans, validFloorPlanRequests);
    await _context.SaveChangesAsync();

    // Check and return errors if any
    if (errorFloorPlanIds.Any() || errorFloorPlanItemIds.Any() || errorFloorPlanCommentIds.Any())
    {
      var validationErrors = new List<ValidationError>();

      if (errorFloorPlanIds.Any())
      {
        validationErrors.Add(new ValidationError($"Failed to create floor plans with IDs: {string.Join(", ", errorFloorPlanIds)}"));
      }
      if (errorFloorPlanItemIds.Any())
      {
        validationErrors.Add(new ValidationError($"Failed to create items with IDs: {string.Join(", ", errorFloorPlanItemIds)}"));
      }
      if (errorFloorPlanCommentIds.Any())
      {
        validationErrors.Add(new ValidationError($"Failed to create comments with IDs: {string.Join(", ", errorFloorPlanCommentIds)}"));
      }

      return Result<bool>.Invalid(validationErrors);
    }

    return Result.Success(true);
  }

  public async Task SaveFloorPlanFilesAsync(List<FloorPlan> floorPlans, List<FloorPlanCommand> floorPlanRequests)
  {
    foreach (var (floorPlan, f) in floorPlans.Zip(floorPlanRequests))
    {
      if (f.Files?.Any() ?? false)
      {
        var savedFiles = await _fileService.SaveFilesAsync(floorPlan.WarehouseId, floorPlan.Id, f.Files);
        var floorPlanFiles = savedFiles
            .Select(sf => new FloorPlanFiles
            {
              Id = Guid.NewGuid(),
              FloorPlanId = floorPlan.Id,
              FileName = sf.FileName,
              FileUrl = sf.FilePath,
              ContentType = sf.ContentType
            })
            .ToList();

        await _context.FloorPlanFiles.AddRangeAsync(floorPlanFiles);
      }
    }
  }
}
