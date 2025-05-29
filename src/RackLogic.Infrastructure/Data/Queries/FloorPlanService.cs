using System.Text.RegularExpressions;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.FloorPlanItems;
using RackLogic.UseCases.FloorPlanItems.GetDetail;
using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.List;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.Infrastructure.Data.Queries;
public class FloorPlanService : IFloorPlanService
{
  private readonly AppDbContext _context;
  private readonly IFileService _fileService;
  private readonly IRepositoryWithExpression<BayConfiguration> _bayConfigurationRepository;
  public FloorPlanService(AppDbContext context,
                          IFileService fileService,
                          IRepositoryWithExpression<BayConfiguration> bayConfigurationRepository)
  {
    _context = context;
    _fileService = fileService;
    _bayConfigurationRepository = bayConfigurationRepository;
  }

  //Save files for FloorPlan
  public async Task<List<FloorPlanFiles>> SaveFloorPlanFilesAsync(List<FloorPlanCommand> floorPlanRequests, Guid warehouseId)
  {
    var floorPlanFiles = new List<FloorPlanFiles>();
    foreach (var f in floorPlanRequests)
    {
      if (f.Files?.Any() ?? false)
      {
        var savedFiles = await _fileService.SaveFilesAsync(warehouseId, f.FloorPlanId, f.Files);
        floorPlanFiles.AddRange(savedFiles
            .Select(sf => new FloorPlanFiles
            {
              Id = Guid.NewGuid(),
              FloorPlanId = f.FloorPlanId,
              FileName = sf.FileName,
              FileUrl = sf.FilePath,
              ContentType = sf.ContentType
            }));
      }
    }
    return floorPlanFiles;
  }

  //Validate BayConfigurationId And DefaultSettings FloorPlan
  public async Task<Result<bool>> ValidateBayConfigurationIdAndDefaultSettingsAsync(
      List<FloorPlanCommand> floorPlans,
      Guid warehouseId,
      CancellationToken cancellationToken)
  {
    var validBayConfigIds = await _bayConfigurationRepository
        .FindAsync(bc => bc.WarehouseId == warehouseId, cancellationToken);
    var validBayConfigIdSet = new HashSet<string>(validBayConfigIds.Select(bc => bc.BayConfigId));

    if (!validBayConfigIdSet.Any())
      return Result.Invalid(new ValidationError("No valid BayConfigId for this WarehouseId."));

    var invalidItems = floorPlans
        .SelectMany(fp => fp.Items)
        .Where(item => !string.IsNullOrEmpty(item.BayConfigurationId) && !validBayConfigIdSet.Contains(item.BayConfigurationId));

    if (invalidItems.Any())
      return Result.Invalid(new ValidationError($"Invalid BayConfigId(s): {string.Join(", ", invalidItems.Select(i => i.BayConfigurationId))} in FloorPlanItems."));

    var invalidSetting = floorPlans
        .SelectMany(fp => new[]
        {
                new { Name = BayConfigurationCategory.SingleBin.ToString(), Value = fp.DefaultSettings?.SingleBin },
                new { Name = BayConfigurationCategory.SingleBinDoubleDeep.ToString(), Value = fp.DefaultSettings?.SingleBinDoubleDeep },
                new { Name = BayConfigurationCategory.PairSingleBin.ToString(), Value = fp.DefaultSettings?.PairSingleBin },
                new { Name = BayConfigurationCategory.PairSingleBinDoubleDeep.ToString(), Value = fp.DefaultSettings?.PairSingleBinDoubleDeep },
                new { Name = BayConfigurationCategory.DoubleBin.ToString(), Value = fp.DefaultSettings?.DoubleBin },
                new { Name = BayConfigurationCategory.DoubleBinDoubleDeep.ToString(), Value = fp.DefaultSettings?.DoubleBinDoubleDeep },
                new { Name = BayConfigurationCategory.PairDoubleBin.ToString(), Value = fp.DefaultSettings?.PairDoubleBin },
                new { Name = BayConfigurationCategory.PairDoubleBinDoubleDeep.ToString(), Value = fp.DefaultSettings?.PairDoubleBinDoubleDeep },
                new { Name = BayConfigurationCategory.TripleBin.ToString(), Value = fp.DefaultSettings?.TripleBin },
                new { Name = BayConfigurationCategory.TripleBinDoubleDeep.ToString(), Value = fp.DefaultSettings?.TripleBinDoubleDeep },
                new { Name = BayConfigurationCategory.PairTripleBin.ToString(), Value = fp.DefaultSettings?.PairTripleBin },
                new { Name = BayConfigurationCategory.PairTripleBinDoubleDeep.ToString(), Value = fp.DefaultSettings?.PairTripleBinDoubleDeep },
        })
        .FirstOrDefault(ds => !string.IsNullOrEmpty(ds.Value) && !validBayConfigIdSet.Contains(ds.Value));

    if (invalidSetting != null)
      return Result.Invalid(new ValidationError($"Invalid BayConfigId for {invalidSetting.Name} in DefaultSettings."));

    return Result.Success(true);
  }

  public async Task<List<FloorPlanDTO>> GetFloorPlansWithFilesByWarehouseIdAsync(Guid warehouseId)
  {
    var bayConfigurations = await _context.BayConfigurations
        .Where(bc => bc.WarehouseId == warehouseId)
        .ToListAsync();

    //Get list FloorPlans by WarehouseId
    var floorPlans = await _context.FloorPlans
        .Include(f => f.FloorPlanFiles)
        .Include(f => f.FloorPlanItems)
        .ThenInclude(i => i.Comments)
        .Include(f => f.DefaultSettings)
        .Where(f => f.WarehouseId == warehouseId)
        .ToListAsync();

    return floorPlans.Select(floorPlan => floorPlan.ToFloorPlanDTO(bayConfigurations)).ToList();
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
    var floorPlansToCreateList = floorPlansToCreate.ToList();
    var createResult = await CreateNewFloorPlansAsync(floorPlansToCreateList, warehouseId, userCreatedId);
    if (!createResult.IsSuccess) return Result<bool>.Invalid(createResult.ValidationErrors);

    // Delete FloorPlans that are not in the client
    if (floorPlansToDelete.Any())
    {
      _context.FloorPlans.RemoveRange(floorPlansToDelete);
    }

    try
    {
      // Save all changes to the database
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
      var errorMessage = ex?.InnerException?.Message;
      var match = Regex.Match(errorMessage ?? "", @"\(([^)]+)\)");
      if (match.Success && Guid.TryParse(match.Groups[1].Value, out Guid duplicateId))
      {
        return Result.Invalid(new ValidationError($"Duplicate Id: {duplicateId}"));
      }
      throw;
    }

    return Result.Success(true);
  }

  private async Task<Result<bool>> CreateNewFloorPlansAsync(List<UpdateFloorPlanInformationCommand> floorPlansToCreate, Guid warehouseId, Guid userCreatedId)
  {
    if (!floorPlansToCreate.Any()) return Result.Success(true);

    var bayConfigurations = await _bayConfigurationRepository.FindAsync(f => f.WarehouseId == warehouseId);
    //Map floorPlansToCreate to FloorPlanCommand
    var floorPlansCommands = floorPlansToCreate
     .Select(fp => fp.ToFloorPlanCommand())
     .ToList();

    //Mapping FloorPlan and FloorPlanFiles
    var floorPlans = floorPlansToCreate.Select(fp => fp.ToUpdateFloorPlan(warehouseId, bayConfigurations)).ToList();
    //Save Files
    var floorPlanFiles = await SaveFloorPlanFilesAsync(floorPlansCommands, warehouseId);
    floorPlans = floorPlans.MapFloorPlanFilesToFloorPlan(floorPlanFiles);

    await _context.FloorPlans.AddRangeAsync(floorPlans);

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

    foreach (var plan in existFloorAssetsDb)
    {
      var processResult = await ProcessFloorPlanAsync(plan, warehouseId);
      if (!processResult.IsSuccess) return processResult;
    }

    return Result.Success(true);
  }

  // Update FloorPlanAsset
  private async Task<Result<bool>> ProcessFloorPlanAsync(UpdateFloorPlanInformationCommand plan, Guid warehouseId)
  {
    var bayConfigurations = await _bayConfigurationRepository.FindAsync(f => f.WarehouseId == warehouseId);
    var floorPlan = await _context.FloorPlans
        .Include(fp => fp.DefaultSettings)
        .FirstOrDefaultAsync(x => x.Id == plan.FloorPlanId);
    if (floorPlan is null) return Result.Invalid(new ValidationError($"Update failed: FloorPlan with ID {plan.FloorPlanId} not found in database."));

    plan.UpdateFloorPlan(floorPlan, bayConfigurations);

    var itemResult = await UpdateFloorPlanItemsAsync(plan.Items, floorPlan.Id, bayConfigurations);
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
  private async Task<Result<bool>> UpdateFloorPlanItemsAsync(IEnumerable<UpdateFloorPlanItemInformationCommand> items, Guid floorPlanId, IEnumerable<BayConfiguration> bayConfigurations)
  {
    if (items is null || !items.Any()) return Result.Success(true);

    // Function to get the list Items and Comments in Db
    var (floorPlanItemsDb, floorPlanCommentsDb) = await GetFloorPlanRelatedEntitiesAsync();

    var floorPlanItemsIds = floorPlanItemsDb.Select(item => item.Id);
    var floorPlanCommentsIds = floorPlanCommentsDb.Select(comment => comment.Id);

    // Get item Ids from the request list
    var itemIds = items.Select(i => i.Id);
    // Get the current FloorPlanItems in the Db with ID in itemIds
    var floorPlanItems = floorPlanItemsDb.Where(i => i.FloorPlanId == floorPlanId);

    // 1. Delete Items that are in Db but not in client
    var itemIdsToDelete = floorPlanItemsIds.Except(itemIds);
    if (itemIdsToDelete.Any())
    {
      _context.FloorPlanItems.RemoveRange(floorPlanItems.Where(i => itemIdsToDelete.Contains(i.Id)));
    }

    // Existing ids and missing ids to be added
    var existingItemIds = floorPlanItems.Select(i => i.Id).ToHashSet();
    var missingItemIds = itemIds.Except(existingItemIds);

    // 2. If itemId is available in Db and client: normal update.
    var updatedItems = items
        .Where(item => existingItemIds.Contains(item.Id))
        .Join(floorPlanItems, item => item.Id, floorPlanItem => floorPlanItem.Id, (item, floorPlanItem) => new { item, floorPlanItem })
        .ToList();

    updatedItems.ForEach(x => x.item.UpdateFloorPlanItem(x.floorPlanItem, bayConfigurations));

    var floorPlanItemsToUpdate = updatedItems.Select(x => x.floorPlanItem);
    _context.FloorPlanItems.UpdateRange(floorPlanItemsToUpdate);

    // 3. Add new Items that are not in the Db
    var newItems = items
        .Where(item => missingItemIds.Contains(item.Id))
        .Select(item => item.ToCreateFloorPlanItem(floorPlanId, bayConfigurations));

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

    foreach (var x in updatedItems.Where(x => x.item.Comments?.Any() == true).ToList())
    {
      var result = await UpdateFloorPlanItemCommentsAsync(x.item.Comments, x.floorPlanItem.Id);
      if (!result.IsSuccess) return result;
    }

    return Result.Success(true);
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

    return Result.Success(true);
  }

  public async Task<FloorPlanDTO?> GetDetailAsync(Guid wareHouseId, Guid floorPlanId, CancellationToken cancellationToken = default)
  {
    var bayConfigurations = await _context.BayConfigurations
        .Where(bc => bc.WarehouseId == wareHouseId)
        .ToListAsync();

    var floorPlan = await _context.FloorPlans
        .Include(f => f.FloorPlanFiles)
        .Include(f => f.FloorPlanItems).ThenInclude(i => i.Comments)
        .Include(f => f.FloorPlanItems).ThenInclude(i => i.DefectLogs)
        .Include(f => f.DefaultSettings)
        .FirstOrDefaultAsync(f => f.Id == floorPlanId);

    if (floorPlan == null)
      return null;

    return floorPlan.ToFloorPlanDTO(bayConfigurations);
  }

  public async Task<IEnumerable<FloorPlanItemDto>?> GetDetailDefectLogAsync(Guid floorPlanId, CancellationToken cancellationToken)
  {
    var floorPlanItems = await _context.FloorPlanItems
        .Include(i => i.DefectLogs!).ThenInclude(d => d.Component)
        .Where(i => i.FloorPlanId == floorPlanId)
        .ToListAsync(cancellationToken);

    return floorPlanItems.Select(x => x.Mapping()).ToList();
  }

  public async Task<IEnumerable<FloorPlanSimpleDto>> GetListAsync(GetListFloorPlanQuery request, CancellationToken cancellationToken)
  {
    var floorPlans = await _context.FloorPlans
        .Where(f => f.WarehouseId == request.wareHouseId)
        .OrderBy(f => f.AssetName)
        .Select(f => f.Mapping())
        .ToListAsync();

    return floorPlans;
  }
}
