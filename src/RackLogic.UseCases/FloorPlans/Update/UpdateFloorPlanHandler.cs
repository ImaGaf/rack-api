using System.Security.Claims;
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

    var results = new List<bool>();

    foreach (var plan in request.FloorAsset)
    {
      results.Add(await ProcessFloorPlan(plan, request.WarehouseId));
    }

    return !results.Any(x => x == false);
  }

  //Funcition
  private async Task<bool> ProcessFloorPlan(UpdateFloorPlanInformationCommand plan, Guid warehouseId)
  {
    var floorPlan = await _floorPlanRepository.FirstOrDefaultAsync(x => x.Id == plan.FloorPlanId);
    if (floorPlan is null) return false;

    plan.UpdateFloorPlan(floorPlan);
    await UpdateFloorPlanItems(plan.Items);
    await HandleFileOperations(plan, warehouseId, floorPlan.Id);
    await _floorPlanRepository.UpdateAsync(floorPlan);

    return true;
  }

  private async Task UpdateFloorPlanItems(List<UpdateFloorPlanItemInformationCommand> items)
  {
    var itemIds = items.Select(i => i.Id).ToList();
    var floorPlanItems = await _floorPlanItemRepository.ListAsync(i => itemIds.Contains(i.Id));

    var updatedItems = items
        .Join(floorPlanItems, item => item.Id, floorPlanItem => floorPlanItem.Id, (item, floorPlanItem) => new { item, floorPlanItem })
        .ToList();

    updatedItems.ForEach(x => x.item.UpdateFloorPlanItem(x.floorPlanItem));

    // Process comments at one time
    var commentTasks = updatedItems
        .Where(x => x.item.Comments != null && x.item.Comments.Any())
        .Select(x => UpdateFloorPlanItemComments(x.item.Comments, x.floorPlanItem.Id));

    await Task.WhenAll(commentTasks);
  }

  private async Task UpdateFloorPlanItemComments(List<UpdateFloorPlanCommentCommand>? comments, Guid floorPlanItemId)
  {
    if (comments == null || comments.Count == 0) return;

    var commentIds = comments.Select(c => c.Id);
    var existingComments = await _floorPlanCommentsRepository
        .ListAsync(c => c.FloorPlanItemId == floorPlanItemId && commentIds.Any(x => x == c.Id));

    // Use Join to match comments from requests with entities in DB
    var updatedComments = comments
        .Join(existingComments,
              c => c.Id,
              ec => ec.Id,
              (c, ec) => new { Command = c, Entity = ec })
        .ToList();

    updatedComments.ForEach(pair => pair.Command.UpdateFloorPlanComment(pair.Entity));
  }
  private async Task HandleFileOperations(UpdateFloorPlanInformationCommand plan, Guid warehouseId, Guid floorPlanId)
  {
    if (plan.FileUrlsToDelete is not null && plan.FileUrlsToDelete.Any())
      await DeleteFiles(plan.FileUrlsToDelete);

    if (plan.FilesToUpdate is not null && plan.FilesToUpdate.Any())
      await UpdateExistingFiles(plan.FilesToUpdate, warehouseId, floorPlanId);

    if (plan.NewFiles is not null && plan.NewFiles.Any())
      await SaveNewFiles(plan.NewFiles, warehouseId, floorPlanId);
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

      if (fileRecord is null) continue;
    }
  }

  private async Task SaveNewFiles(List<IFormFile> newFiles, Guid warehouseId, Guid floorPlanId)
  {
    var savedFiles = await _fileService.SaveFilesAsync(warehouseId, floorPlanId, newFiles);

    var floorPlanFiles = savedFiles.Select(f => new FloorPlanFiles
    {
      Id = Guid.NewGuid(),
      FloorPlanId = floorPlanId,
      FileName = f.FileName,
      FileUrl = f.FilePath,
      ContentType = f.ContentType
    }).ToList();

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
