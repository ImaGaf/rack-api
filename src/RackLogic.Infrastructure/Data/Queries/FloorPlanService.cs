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
        .Select(floorPlanComment => floorPlanComment.Id);

    var errorFloorPlanItemIdsFromComments = floorPlanComments
        .Where(comment => errorFloorPlanCommentIds.Contains(comment.Id))
        .Select(comment => comment.FloorPlanItemId)
        .Distinct();

    errorFloorPlanItemIds.AddRange(errorFloorPlanItemIdsFromComments.Except(errorFloorPlanItemIds));

    return errorFloorPlanCommentIds.ToList();
  }

  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlanItemsAsync(List<FloorPlanItem> floorPlanItems, List<Guid> errorFloorPlanIds)
  {
    var floorPlanItemIds = floorPlanItems.Select(f => f.Id).ToHashSet();
    var existingFloorPlanItemIds = (await _context.FloorPlanItems.Where(f => floorPlanItemIds.Contains(f.Id)).ToListAsync()).Select(e => e.Id).ToHashSet();

    var errorFloorPlanIdsSet = errorFloorPlanIds.ToHashSet();

    var errorFloorPlanItemIds = floorPlanItems
        .Where(floorPlanItem => errorFloorPlanIdsSet.Contains(floorPlanItem.FloorPlanId) || existingFloorPlanItemIds.Contains(floorPlanItem.Id))
        .Select(floorPlanItem => floorPlanItem.Id);

    errorFloorPlanIds.AddRange(floorPlanItems.Where(floorPlanItem => errorFloorPlanItemIds.Contains(floorPlanItem.Id)).Select(floorPlanItem => floorPlanItem.FloorPlanId).Distinct().Except(errorFloorPlanIds));

    return errorFloorPlanItemIds.ToList();
  }

  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlansAsync(List<FloorPlan> floorPlans)
  {
    var floorPlanIds = floorPlans.Select(f => f.Id);
    var existingFloorPlanIds = await _context.FloorPlans
        .Where(f => floorPlanIds.Contains(f.Id))
        .Select(f => f.Id)
        .ToListAsync();
    return existingFloorPlanIds;
  }

  private (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanDataForUpdate(IEnumerable<UpdateFloorPlanInformationCommand> notExistFloorAssetsDb, Guid warehouseId, Guid userCreatedId)
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

    // Check valid floorPlans
    var validFloorPlans = floorPlans
        .Where(f =>
        {
          var items = floorPlanItems.Where(i => i.FloorPlanId == f.Id).ToList();
          return !errorFloorPlanIds.Contains(f.Id)
             && items.Any()
             && items.All(i => !errorFloorPlanItemIds.Contains(i.Id));
        })
        .ToList();

    // Check valid floorPlanItems
    var validFloorPlanItems = floorPlanItems
        .Where(i => !errorFloorPlanItemIds.Contains(i.Id) && validFloorPlans.Any(f => f.Id == i.FloorPlanId))
        .ToList();

    // Check valid floorPlanComments
    var validFloorPlanComments = floorPlanComments
        .Where(c => !errorFloorPlanCommentIds.Contains(c.Id) && validFloorPlanItems.Any(i => i.Id == c.FloorPlanItemId))
        .ToList();

    // Update errorFloorPlanIds
    errorFloorPlanIds = floorPlans
        .Select(f => f.Id)
        .Except(validFloorPlans.Select(f => f.Id))
        .ToList();

    // Save valid data to DB
    await _context.FloorPlans.AddRangeAsync(validFloorPlans);
    await _context.FloorPlanItems.AddRangeAsync(validFloorPlanItems);
    await _context.FloorPlanComments.AddRangeAsync(validFloorPlanComments);

    // Save only valid files
    var validFloorPlanRequests = floorPlanRequests
    .Where(f => validFloorPlans.Any(vf => vf.Id == f.FloorPlanId))
    .ToList();

    await SaveFloorPlanFilesAsync(validFloorPlans, validFloorPlanRequests);

    // Check and return errors
    if (errorFloorPlanIds.Any())
    {
      return Result<bool>.Invalid(new List<ValidationError> { new ValidationError($"Failed to create floor plans with IDs: {string.Join(", ", errorFloorPlanIds)}") });
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
  }

  public async Task<Result<bool>> CreateOrUpdateFloorAsssetsAsync(List<UpdateFloorPlanInformationCommand> floorAssets, Guid warehouseId, Guid userCreatedId)
  {
    // Sort the list of FloorPlans to be processed
    var (floorPlansToUpdate, floorPlansToCreate, floorPlansToDelete) = await GetFloorPlanCategoriesAsync(floorAssets, warehouseId);

    // Update existing FloorPlans in Db
    var updateResult = await ProcessExistingFloorPlansAsync(floorPlansToUpdate, warehouseId);
    if (!updateResult.IsSuccess) return Result<bool>.Invalid(updateResult.ValidationErrors);

    // Create new FloorPlans that not in the Db
    var createResult = await CreateNewFloorPlansAsync(floorPlansToCreate, warehouseId, userCreatedId);
    if (!createResult.IsSuccess) return Result<bool>.Invalid(createResult.ValidationErrors);

    // Delete FloorPlans that are not in the client
    if (floorPlansToDelete.Any())
    {
      _context.FloorPlans.RemoveRange(floorPlansToDelete);
    }

    await _context.SaveChangesAsync();
    return Result.Success(true);
  }

  // Get list of FloorPlans by warehouseId
  private async Task<List<FloorPlan>> GetExistingFloorPlansAsync(Guid warehouseId)
  {
    return await _context.FloorPlans
        .Where(f => f.WarehouseId == warehouseId)
        .ToListAsync();
  }

  private async Task<(IEnumerable<UpdateFloorPlanInformationCommand> floorPlansToUpdate,
                    IEnumerable<UpdateFloorPlanInformationCommand> floorPlansToCreate,
                    IEnumerable<FloorPlan> floorPlansToDelete)>
    GetFloorPlanCategoriesAsync(IEnumerable<UpdateFloorPlanInformationCommand> floorAssets, Guid warehouseId)
  {
    var existingFloorPlansDb = await GetExistingFloorPlansAsync(warehouseId);
    var existingFloorPlanIds = existingFloorPlansDb.Select(f => f.Id).ToHashSet();

    // Find FloorPlans that already exist in the Db and need to be updated
    var floorPlansToUpdate = floorAssets.Where(p => existingFloorPlanIds.Contains(p.FloorPlanId));
    var floorPlansToCreate = floorAssets.Where(p => !existingFloorPlanIds.Contains(p.FloorPlanId));

    // Find FloorPlans in Db but not in client, need to delete
    var floorPlansToDelete = existingFloorPlansDb
        .Where(dbFloorPlan => !floorAssets.Any(fa => fa.FloorPlanId == dbFloorPlan.Id));

    return (floorPlansToUpdate, floorPlansToCreate, floorPlansToDelete);
  }

  private async Task<Result<bool>> ProcessExistingFloorPlansAsync(IEnumerable<UpdateFloorPlanInformationCommand> existFloorAssetsDb, Guid warehouseId)
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

  // Update FloorPlanAsset
  private async Task<Result<bool>> ProcessFloorPlanAsync(UpdateFloorPlanInformationCommand plan, Guid warehouseId)
  {
    var floorPlan = await _floorPlanRepository.FirstOrDefaultAsync(x => x.Id == plan.FloorPlanId);
    if (floorPlan is null)  return Result.Invalid(new ValidationError($"Update failed: FloorPlan with ID {plan.FloorPlanId} not found in database."));

    plan.UpdateFloorPlan(floorPlan);

    var itemResult = await UpdateFloorPlanItemsAsync(plan.Items, floorPlan.Id);
    if (!itemResult.IsSuccess) return itemResult;

    await HandleFileOperationsAsync(plan, warehouseId, floorPlan.Id);
    return Result.Success(true);
  }

  // Get list of Ids of FloorPlanItems and FloorPlanComments in Db
  private async Task<(List<FloorPlanItem> floorPlanItems, List<FloorPlanComments> floorPlanComments)> GetFloorPlanRelatedEntitiesAsync()
  {
    var floorPlanItems = await _context.FloorPlanItems
        .AsNoTracking()
        .ToListAsync();

    var floorPlanComments = await _context.FloorPlanComments
        .AsNoTracking()
        .ToListAsync();

    return (floorPlanItems, floorPlanComments);
  }

  // Update FloorPlanItems list
  private async Task<Result<bool>> UpdateFloorPlanItemsAsync(IEnumerable<UpdateFloorPlanItemInformationCommand> items, Guid floorPlanId)
  {
    if (items is null || !items.Any()) return Result.Success(true);

    // Function to get the list Items and Comments in Db
    var (floorPlanItemsDb, floorPlanCommentsDb) = await GetFloorPlanRelatedEntitiesAsync();

    var floorPlanItemsIds = floorPlanItemsDb.Select(item => item.Id);
    var floorPlanCommentsIds = floorPlanCommentsDb.Select(comment => comment.Id);

    // Get itemIds from the upload list
    var itemIds = items.Select(i => i.Id);
    // Get the current FloorPlanItems in the Db with ID in itemIds
    var floorPlanItems = floorPlanItemsDb.Where(i => i.FloorPlanId == floorPlanId && itemIds.Contains(i.Id));

    // 1. Delete Items that are in Db but not in client
    var itemIdsToDelete = floorPlanItemsIds.Except(itemIds);
    if (itemIdsToDelete.Any())
    {
      _context.FloorPlanItems.RemoveRange(
          floorPlanItems.Where(i => itemIdsToDelete.Contains(i.Id))
      );
    }

    // Existing ids and missing ids to be added
    var existingItemIds = floorPlanItems.Select(i => i.Id).ToHashSet();
    var missingItemIds = itemIds.Except(existingItemIds);

    // 2. If itemId is available in Db and client: normal update.
    var updatedItems = items
        .Where(item => existingItemIds.Contains(item.Id))
        .Join(floorPlanItems, item => item.Id, floorPlanItem => floorPlanItem.Id, (item, floorPlanItem) => new { item, floorPlanItem })
        .ToList();

    updatedItems.ForEach(x => x.item.UpdateFloorPlanItem(x.floorPlanItem));

    // 3. Add new Items that are not in the Db
    var newItems = items
        .Where(item => missingItemIds.Contains(item.Id))
        .Select(item => item.ToCreateFloorPlanItem(floorPlanId));

    // Check if new itemId is already in the floorPlanItemsIds already in the Db
    if (newItems.Any(i => floorPlanItemsIds.Contains(i.Id)))
    {
      var conflictingItemIds = newItems.Select(i => i.Id).Where(floorPlanItemsIds.Contains).ToList();
      return Result<bool>.Invalid(conflictingItemIds.Select(id => new ValidationError($"Item with ID {id} already exists in another FloorPlan.")));
    }

    // Get all CommentIds in newItems, handle when Comments can be null
    var newCommentIds = newItems
        .SelectMany(item => item.Comments?.Select(c => c.Id) ?? Enumerable.Empty<Guid>());

    // Check if commentId already exists in Db
    var conflictingCommentIds = newCommentIds.Where(floorPlanCommentsIds.Contains);

    if (conflictingCommentIds.Any())
    {
      var commentError = conflictingCommentIds.Select(id => new ValidationError($"Comment with ID {id} already exists."));
      return Result<bool>.Invalid(commentError);
    }

    if (newItems.Any())
    {
      await _context.FloorPlanItems.AddRangeAsync(newItems);
    }

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

    return commentErrors.Any() ? Result<bool>.Invalid(commentErrors) : Result.Success(true);
  }

  private async Task<Result<bool>> UpdateFloorPlanItemCommentsAsync(List<UpdateFloorPlanCommentCommand>? comments, Guid floorPlanItemId)
  {
    if (comments == null || !comments.Any()) return Result.Success(true);

    var commentIds = comments.Select(c => c.Id).ToList();

    // Get current comments in DB for floorPlanItemId
    var existingComments = await _context.FloorPlanComments
        .Where(c => c.FloorPlanItemId == floorPlanItemId && commentIds.Contains(c.Id))
        .ToListAsync();

    var existingCommentIds = existingComments.Select(c => c.Id).ToHashSet();
    var missingCommentIds = commentIds.Except(existingCommentIds);

    var validationErrors = new List<ValidationError>();

    // Update valid comments
    var updatedComments = comments
        .Where(c => existingCommentIds.Contains(c.Id))
        .Join(existingComments, c => c.Id, ec => ec.Id, (c, ec) => new { Command = c, Entity = ec })
        .ToList();

    updatedComments.ForEach(pair => pair.Command.UpdateFloorPlanComment(pair.Entity));

    // Delete Comments that are in Db but not in client
    var commentIdsToDelete = existingCommentIds.Except(commentIds);
    if (commentIdsToDelete.Any())
    {
      _context.FloorPlanComments.RemoveRange(
          existingComments.Where(c => commentIdsToDelete.Contains(c.Id))
      );
    }

    // Add new Comments that are not in the Db
    var newComments = comments.Where(c => missingCommentIds.Contains(c.Id))
                              .Select(c => c.ToFloorPlanComment(floorPlanItemId));

    // Check if new Comment ID already exists in DB
    var newCommentIds = newComments.Select(c => c.Id).ToHashSet();
    var existingCommentConflicts = await _context.FloorPlanComments
        .Where(c => newCommentIds.Contains(c.Id))
        .Select(c => c.Id)
        .ToListAsync();

    if (existingCommentConflicts.Any())
    {
      var commentError = existingCommentConflicts.Select(id => new ValidationError($"Comment with ID {id} already exists in the database."));
      return Result<bool>.Invalid(commentError);
    }

    if (newComments.Any())
    {
      await _context.FloorPlanComments.AddRangeAsync(newComments);
    }

    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  }

  private async Task<Result<bool>> CreateNewFloorPlansAsync(IEnumerable<UpdateFloorPlanInformationCommand> notExistFloorAssetsDb, Guid warehouseId, Guid userCreatedId)
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

    if (!saveResult.IsSuccess) return saveResult;

    await _context.SaveChangesAsync();
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
