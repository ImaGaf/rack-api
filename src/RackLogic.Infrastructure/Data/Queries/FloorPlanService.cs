using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
  private readonly IRepositoryWithExpression<FloorPlan> _floorPlanRepository;
  public FloorPlanService(AppDbContext context, 
                          IFileService fileService, 
                          IRepositoryWithExpression<FloorPlan> floorPlanRepository)
  {
    _context = context;
    _fileService = fileService;
    _floorPlanRepository = floorPlanRepository;
  }

  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlanCommentsAsync(List<FloorPlanComments> floorPlanComments, List<Guid> errorFloorPlanItemIds)
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

  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlanItemsAsync(List<FloorPlanItem> floorPlanItems, List<Guid> errorFloorPlanIds)
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

  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlansAsync(List<FloorPlan> floorPlans)
  {
    var floorPlanIds = floorPlans.Select(f => f.Id).ToList();
    var existingFloorPlans = await _context.FloorPlans.Where(f => floorPlanIds.Contains(f.Id)).ToListAsync();
    return existingFloorPlans.Select(f => f.Id).ToList();
  }

  private (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanDataForUpdate( List<UpdateFloorPlanInformationCommand> notExistFloorAssetsDb, Guid warehouseId, Guid userCreatedId)
  {
    var floorPlans = notExistFloorAssetsDb
        .Select(f => f.ToFloorPlan(warehouseId, userCreatedId))
        .ToList();

    var floorPlanItems = notExistFloorAssetsDb
        .SelectMany(f => f.Items, (f, item) => item.ToFloorPlanItem(f.FloorPlanId)) 
        .ToList();

    var floorPlanComments = notExistFloorAssetsDb
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

  private async Task<Result<bool>> SaveFloorPlanDataAsync(List<FloorPlan> floorPlans, List<FloorPlanItem> floorPlanItems, List<FloorPlanComments> floorPlanComments, List<FloorPlanCommand> floorPlanRequests)
  {
    // Check errors for each object
    var errorFloorPlanIds = await CheckAndGetErrorsForFloorPlansAsync(floorPlans);
    var errorFloorPlanItemIds = await CheckAndGetErrorsForFloorPlanItemsAsync(floorPlanItems, errorFloorPlanIds);
    var errorFloorPlanCommentIds = await CheckAndGetErrorsForFloorPlanCommentsAsync(floorPlanComments, errorFloorPlanItemIds);

    // Check valid floorPlanItems
    var validFloorPlanItems = floorPlanItems.Where(i => !errorFloorPlanItemIds.Contains(i.Id)).ToList();

    // Check valid floorPlans
    var validFloorPlans = floorPlans
        .Where(f => !errorFloorPlanIds.Contains(f.Id) && validFloorPlanItems.Any(i => i.FloorPlanId == f.Id))
        .ToList();

    // If no valid FloorPlan is created, return an error.
    if (!validFloorPlans.Any())
    {
      return Result<bool>.Invalid(new List<ValidationError>
        {
            new ValidationError("Failed to create new valid FloorPlans with invalid items.")
        });
    }
    //Save valid floorPlans to DB
    await _context.FloorPlans.AddRangeAsync(validFloorPlans);
    //Save valid floorPlanItems to DB
    await _context.FloorPlanItems.AddRangeAsync(validFloorPlanItems);

    // Save valid floorPlanComments to DB
    var validFloorPlanComments = floorPlanComments.Where(c => !errorFloorPlanCommentIds.Contains(c.Id)).ToList();
    await _context.FloorPlanComments.AddRangeAsync(validFloorPlanComments);

    // Save only valid files
    var validFloorPlanRequests = floorPlanRequests
    .Where(f => validFloorPlans.Any(vf => vf.Id == f.FloorPlanId))
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

  private async Task SaveFloorPlanFilesAsync(List<FloorPlan> floorPlans, List<FloorPlanCommand> floorPlanRequests)
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

  // Update FloorPlanAssets list
  private async Task<Result<bool>> ProcessFloorPlanAsync(UpdateFloorPlanInformationCommand plan, Guid warehouseId)
  {
    var floorPlan = await _floorPlanRepository.FirstOrDefaultAsync(x => x.Id == plan.FloorPlanId);
    if (floorPlan is null)
      return Result.Invalid(new ValidationError($"Update failed: FloorPlan with ID {plan.FloorPlanId} not found in database."));

    plan.UpdateFloorPlan(floorPlan);

    var itemResult = await UpdateFloorPlanItemsAsync(plan.Items);
    if (!itemResult.IsSuccess) return itemResult;

    await HandleFileOperationsAsync(plan, warehouseId, floorPlan.Id);
    await _floorPlanRepository.UpdateAsync(floorPlan);

    return Result.Success(true);
  }

  // Update FloorPlanItems list
  private async Task<Result<bool>> UpdateFloorPlanItemsAsync(List<UpdateFloorPlanItemInformationCommand> items)
  {
    if (items is null || !items.Any()) return Result.Success(true);

    var itemIds = items.Select(i => i.Id).ToList();
    var floorPlanItems = await _context.FloorPlanItems.Where(i => itemIds.Contains(i.Id)).ToListAsync();
    var existingItemIds = floorPlanItems.Select(i => i.Id).ToHashSet();
    var missingItemIds = itemIds.Except(existingItemIds);

    var validationErrors = missingItemIds
        .Select(id => new ValidationError($"Update failed: FloorPlanItem with ID {id} not found in database."))
        .ToList();

    // Update valid items
    var updatedItems = items
        .Where(item => existingItemIds.Contains(item.Id))
        .Join(floorPlanItems, item => item.Id, floorPlanItem => floorPlanItem.Id, (item, floorPlanItem) => new { item, floorPlanItem })
        .ToList();

    updatedItems.ForEach(x => x.item.UpdateFloorPlanItem(x.floorPlanItem));

    // Process Comments of each FloorPlanItem
    var commentErrors = new List<ValidationError>();

    foreach (var x in updatedItems.Where(x => x.item.Comments?.Any() == true).ToList())
    {
      var result = await UpdateFloorPlanItemCommentsAsync(x.item.Comments, x.floorPlanItem.Id);
      if (!result.IsSuccess)
      {
        commentErrors.AddRange(result.ValidationErrors);
      }
    }

    validationErrors.AddRange(commentErrors);

    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  }

  private async Task<Result<bool>> UpdateFloorPlanItemCommentsAsync(List<UpdateFloorPlanCommentCommand>? comments, Guid floorPlanItemId)
  {
    if (comments == null || !comments.Any()) return Result.Success(true);

    var commentIds = comments.Select(c => c.Id).ToList();
    var existingComments = await _context.FloorPlanComments
        .Where(c => c.FloorPlanItemId == floorPlanItemId && commentIds.Contains(c.Id))
        .ToListAsync();

    var existingCommentIds = existingComments.Select(c => c.Id).ToHashSet();
    var missingCommentIds = commentIds.Except(existingCommentIds);

    var validationErrors = missingCommentIds
        .Select(id => new ValidationError($"Update failed: FloorPlanComment with ID {id} not found in database."))
        .ToList();

    // Update valid comments
    var updatedComments = comments
        .Where(c => existingCommentIds.Contains(c.Id))
        .Join(existingComments, c => c.Id, ec => ec.Id, (c, ec) => new { Command = c, Entity = ec })
        .ToList();

    updatedComments.ForEach(pair => pair.Command.UpdateFloorPlanComment(pair.Entity));

    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  }


  // Process files that need to be updated (delete old files, add new files)
  private async Task HandleFileOperationsAsync(UpdateFloorPlanInformationCommand plan, Guid warehouseId, Guid floorPlanId)
  {
    if (plan.FileUrlsToDelete is not null && plan.FileUrlsToDelete.Any())
    {
      await DeleteFilesAsync(plan.FileUrlsToDelete);
    }

    if (plan.Files is not null && plan.Files.Any())
    {
      await SaveNewFilesAsync(plan.Files, warehouseId, floorPlanId);
    }
  }

  // Delete files in database & storage system
  private async Task DeleteFilesAsync(List<string> fileUrlsToDelete)
  {
    await _fileService.DeleteFilesAsync(fileUrlsToDelete);

    var fileNames = fileUrlsToDelete.Select(Path.GetFileName).ToList();
    var fileRecords = await _context.FloorPlanFiles.Where(f => fileNames.Contains(f.FileName)).ToListAsync();

    if (fileRecords.Any())
    {
      _context.FloorPlanFiles.RemoveRange(fileRecords);
      await _context.SaveChangesAsync();
    }
  }

  // Save new file to database
  private async Task SaveNewFilesAsync(List<IFormFile> newFiles, Guid warehouseId, Guid floorPlanId)
  {
    var savedFiles = await _fileService.SaveFilesAsync(warehouseId, floorPlanId, newFiles);

    var floorPlanFiles = savedFiles
       .Select(sf => new FloorPlanFiles
       {
         Id = Guid.NewGuid(),
         FloorPlanId = floorPlanId,
         FileName = sf.FileName,
         FileUrl = sf.FilePath,
         ContentType = sf.ContentType
       }).ToList();

    await _context.FloorPlanFiles.AddRangeAsync(floorPlanFiles);
    await _context.SaveChangesAsync();
  }

  public async Task<Result<bool>> CreateOrUpdateFloorAsssetsAsync(List<UpdateFloorPlanInformationCommand> floorAssets, Guid warehouseId, Guid userCreatedId)
  {
    var validationErrors = new List<ValidationError>();

    var (existFloorAssetsDb, notExistFloorAssetsDb) = await GetExistingAndNewFloorPlansAsync(floorAssets, warehouseId);

    var updateResult = await ProcessExistingFloorPlansAsync(existFloorAssetsDb, warehouseId);
    if (!updateResult.IsSuccess)
    {
      validationErrors.AddRange(updateResult.ValidationErrors);
    }

    var createResult = await CreateNewFloorPlansAsync(notExistFloorAssetsDb, warehouseId, userCreatedId);
    if (!createResult.IsSuccess)
    {
      validationErrors.AddRange(createResult.ValidationErrors);
    }

    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  }

  private async Task<(List<UpdateFloorPlanInformationCommand> existFloorAssetsDb, List<UpdateFloorPlanInformationCommand> notExistFloorAssetsDb)> GetExistingAndNewFloorPlansAsync(
    List<UpdateFloorPlanInformationCommand> floorAssets, Guid warehouseId)
  {
    var existingFloorPlansDb = await _context.FloorPlans
        .Where(f => floorAssets.Select(p => p.FloorPlanId).Contains(f.Id))
        .ToListAsync();

    var existingFloorPlanIds = existingFloorPlansDb.Select(f => f.Id).ToHashSet();
    var existFloorAssetsDb = floorAssets.Where(p => existingFloorPlanIds.Contains(p.FloorPlanId)).ToList();
    var notExistFloorAssetsDb = floorAssets.Where(p => !existingFloorPlanIds.Contains(p.FloorPlanId)).ToList();

    return (existFloorAssetsDb, notExistFloorAssetsDb);
  }

  private async Task<Result<bool>> ProcessExistingFloorPlansAsync(List<UpdateFloorPlanInformationCommand> existFloorAssetsDb, Guid warehouseId)
  {
    if (!existFloorAssetsDb.Any()) return Result.Success(true);

    var validationErrors = new List<ValidationError>();

    foreach (var plan in existFloorAssetsDb)
    {
      var processResult = await ProcessFloorPlanAsync(plan, warehouseId);
      if (!processResult.IsSuccess)
      {
        validationErrors.AddRange(processResult.ValidationErrors);
      }
    }

    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  }

  private async Task<Result<bool>> CreateNewFloorPlansAsync(List<UpdateFloorPlanInformationCommand> notExistFloorAssetsDb, Guid warehouseId, Guid userCreatedId)
  {
    if (!notExistFloorAssetsDb.Any()) return Result.Success(true);

    var (floorPlans, floorPlanItems, floorPlanComments) = GetFloorPlanDataForUpdate(notExistFloorAssetsDb, warehouseId, userCreatedId);
    var floorPlanCommands = notExistFloorAssetsDb.Select(p => p.ToFloorPlanCommand()).ToList();

    var createResult = await SaveFloorPlanDataAsync(floorPlans, floorPlanItems, floorPlanComments, floorPlanCommands);

    return createResult;
  }

  public async Task<Result<bool>> CreateFloorPlanAssetsAsync(List<FloorPlanCommand> FloorPlans, Guid warehouseId, Guid userCreatedId)
  {
    // Get Floor Plan Assets data
    var (floorPlans, floorPlanItems, floorPlanComments) = GetFloorPlanData(FloorPlans, warehouseId, userCreatedId);
    // Save Floor Plan Assets data
    var saveResult = await SaveFloorPlanDataAsync(floorPlans, floorPlanItems, floorPlanComments, FloorPlans);

    if (!saveResult.IsSuccess)
    {
      return Result<bool>.Invalid(saveResult.ValidationErrors);
    }

    return Result.Success(true);
  }


  private (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanData(List<FloorPlanCommand> FloorPlans, Guid warehouseId, Guid userCreatedId)
  {
    var floorPlans = FloorPlans
        .Select(f => f.ToFloorPlan(warehouseId, userCreatedId))
        .ToList();

    var floorPlanItems = FloorPlans
        .SelectMany(f => f.Items, (f, item) => item.ToFloorPlanItem(f.FloorPlanId))
        .ToList();

    var floorPlanComments = FloorPlans
        .SelectMany(f => f.Items)
        .Where(item => item.Comments != null)
        .SelectMany(item => item.Comments ?? new List<FloorPlanCommentCommand>(), (item, comment) => comment.ToFloorPlanComment(item.Id))
        .ToList();

    return (floorPlans, floorPlanItems, floorPlanComments);
  }
}
