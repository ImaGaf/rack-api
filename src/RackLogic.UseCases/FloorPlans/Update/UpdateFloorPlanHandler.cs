using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Update;
public class UpdateFloorPlanHandler(
  IRepositoryWithExpression<FloorPlan> _floorPlanRepository,
  IRepositoryWithExpression<FloorPlanFiles> _floorPlanFilesRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<FloorPlanItem> _floorPlanItemRepository,
  IRepositoryWithExpression<FloorPlanComments> _floorPlanCommentsRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFileService _fileService,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<UpdateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(UpdateFloorPlanCommand request, CancellationToken cancellationToken)
  {

    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    var userCreatedId = request.UserCreatedId;
    if (!await CheckPermissionUserRequest(userCreatedId, request.WarehouseId)) return Result.Forbidden();

    var validationErrors = new List<ValidationError>();

    foreach (var plan in request.FloorAsset)
    {
      var processResult = await ProcessFloorPlan(plan, request.WarehouseId);
      if (!processResult.IsSuccess)
      {
        validationErrors.AddRange(processResult.ValidationErrors);
      }
    }
    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  } 

  //Funcition
  private async Task<Result<bool>> ProcessFloorPlan(UpdateFloorPlanInformationCommand plan, Guid warehouseId)
  {
    var floorPlan = await _floorPlanRepository.FirstOrDefaultAsync(x => x.Id == plan.FloorPlanId);
    if (floorPlan is null) return Result.Invalid(new ValidationError($"Update failed: FloorPlan with ID {plan.FloorPlanId}, not found in database."));

    plan.UpdateFloorPlan(floorPlan);

    var itemResult = await UpdateFloorPlanItems(plan.Items);
    if (!itemResult.IsSuccess) return itemResult;

    await HandleFileOperations(plan, warehouseId, floorPlan.Id);
    await _floorPlanRepository.UpdateAsync(floorPlan);

    return Result.Success(true);
  }

  private async Task<Result<bool>> UpdateFloorPlanItems(List<UpdateFloorPlanItemInformationCommand> items)
  {
    if (items is null || items.Count == 0) return Result.Success(true);

    var itemIds = items.Select(i => i.Id);
    var floorPlanItems = await _floorPlanItemRepository.ListAsync(i => itemIds.Contains(i.Id));
    var existingItemIds = floorPlanItems.Select(i => i.Id);
    var missingItemIds = itemIds.Except(existingItemIds);

    var validationErrors = missingItemIds
        .Select(id => new ValidationError($"Update failed: FloorPlanItem with ID {id}, not found in database."))
        .ToList();

    // Update items valid
    var updatedItems = items
        .Where(item => existingItemIds.Contains(item.Id))
        .Join(floorPlanItems, item => item.Id, floorPlanItem => floorPlanItem.Id, (item, floorPlanItem) => new { item, floorPlanItem })
        .ToList();

    updatedItems.ForEach(x => x.item.UpdateFloorPlanItem(x.floorPlanItem));

    // Function Comments
    var commentErrors = new List<ValidationError>();

    foreach (var x in updatedItems.Where(x => x.item.Comments?.Any() == true).ToList())
    {
      var result = await UpdateFloorPlanItemComments(x.item.Comments, x.floorPlanItem.Id);
      if (!result.IsSuccess)
      {
        commentErrors.AddRange(result.ValidationErrors);
      }
    }

    validationErrors.AddRange(commentErrors);
    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  }

  private async Task<Result<bool>> UpdateFloorPlanItemComments(List<UpdateFloorPlanCommentCommand>? comments, Guid floorPlanItemId)
  {
    if (comments == null || comments.Count == 0) return Result.Success(true);

    var commentIds = comments.Select(c => c.Id).ToList();
    var existingComments = await _floorPlanCommentsRepository
        .ListAsync(c => c.FloorPlanItemId == floorPlanItemId && commentIds.Contains(c.Id));

    var existingCommentIds = existingComments.Select(c => c.Id).ToHashSet();
    var missingCommentIds = commentIds.Where(id => !existingCommentIds.Contains(id));

    var validationErrors = missingCommentIds
        .Select(id => new ValidationError($"Update failed: FloorPlanComment with ID {id}, not found in database."))
        .ToList();

    // Update comments valid
    var updatedComments = comments
        .Where(c => existingCommentIds.Contains(c.Id))
        .Join(existingComments,
              c => c.Id,
              ec => ec.Id,
              (c, ec) => new { Command = c, Entity = ec })
        .ToList();

    updatedComments.ForEach(pair => pair.Command.UpdateFloorPlanComment(pair.Entity));

    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
  }
  private async Task HandleFileOperations(UpdateFloorPlanInformationCommand plan, Guid warehouseId, Guid floorPlanId)
  {
    if (plan.FileUrlsToDelete is not null && plan.FileUrlsToDelete.Any())
    {
      await DeleteFiles(plan.FileUrlsToDelete);
    }

    if (plan.FilesToUpdate is not null && plan.FilesToUpdate.Any())
    {
      await UpdateExistingFiles(plan.FilesToUpdate, warehouseId, floorPlanId);
    }

    if (plan.NewFiles is not null && plan.NewFiles.Any())
    {
      await SaveNewFiles(plan.NewFiles, warehouseId, floorPlanId);
    }
  }

  private async Task DeleteFiles(List<string> fileUrlsToDelete)
  {
    await _fileService.DeleteFilesAsync(fileUrlsToDelete);

    var fileNames = fileUrlsToDelete.Select(Path.GetFileName).ToList();
    var fileRecords = await _floorPlanFilesRepository.ListAsync(f => fileNames.Contains(f.FileName));

    if (fileRecords.Any())
    {
      await _floorPlanFilesRepository.DeleteRangeAsync(fileRecords);
    }
  }

  private async Task UpdateExistingFiles(List<IFormFile> filesToUpdate, Guid warehouseId, Guid floorPlanId)
  {
    var updatedFiles = await _fileService.ReplaceFileAsync(warehouseId, floorPlanId, filesToUpdate);

    foreach (var updatedFile in updatedFiles)
    {
      var fileRecord = await _floorPlanFilesRepository
          .FirstOrDefaultAsync(f => f.FloorPlanId == floorPlanId && f.FileName == updatedFile.FileName);

      if (fileRecord is null)
      {
        continue;
      }
    }
  }

  private async Task SaveNewFiles(List<IFormFile> newFiles, Guid warehouseId, Guid floorPlanId)
  {
    var savedFiles = await _fileService.SaveFilesAsync(warehouseId, floorPlanId, newFiles);

    var floorPlanFiles = savedFiles
       .Select(sf => sf.ToFloorPlanFile(floorPlanId))
       .ToList();

    await _floorPlanFilesRepository.AddRangeAsync(floorPlanFiles);
  }


  private async Task<bool> CheckPermissionUserRequest(Guid userId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises &&
      role.Name != RoleConstant.ClientManager &&
      role.Name != RoleConstant.Inspector) return false;

    return true;
  }
}
