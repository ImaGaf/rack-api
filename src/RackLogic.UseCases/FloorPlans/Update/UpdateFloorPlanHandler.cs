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
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();

    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    var inviteIdString = request.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var userCreatedId = Guid.Parse(inviteIdString);
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

    UpdateFloorPlanDetails(floorPlan, plan);
    await UpdateFloorPlanItems(plan.Items);
    await HandleFileOperations(plan, warehouseId, floorPlan.Id);
    await _floorPlanRepository.UpdateAsync(floorPlan);

    return true;
  }

  private void UpdateFloorPlanDetails(FloorPlan floorPlan, UpdateFloorPlanInformationCommand plan)
  {
    floorPlan.AssetName = plan.Title;
    floorPlan.FloorNumber = plan.Floor;
    floorPlan.Description = plan.Description;
    floorPlan.TotalRack = plan.TotalRack;
    floorPlan.TotalBay = plan.TotalBay;
  }

  private async Task UpdateFloorPlanItems(List<UpdateFloorPlanItemInformationCommand> items)
  {
    foreach (var item in items)
    {
      var floorPlanItem = await _floorPlanItemRepository.FirstOrDefaultAsync(i => i.Id == item.Id);
      if (floorPlanItem is null) continue;

      UpdateFloorPlanItemDetails(floorPlanItem, item);
      await UpdateFloorPlanItemComments(item.Comments, floorPlanItem.Id);
    }
  }

  private void UpdateFloorPlanItemDetails(FloorPlanItem floorPlanItem, UpdateFloorPlanItemInformationCommand item)
  {
    floorPlanItem.Rack = item.Rack;
    floorPlanItem.Bay = item.Bay;
    floorPlanItem.Offset = item.Offset;
    floorPlanItem.Size = item.Size ?? null;
    floorPlanItem.Width = item.Width ?? null;
    floorPlanItem.Height = item.Height ?? null;
    floorPlanItem.ObjectType = item.ObjectType;
    floorPlanItem.Rotation = item.Rotation;
  }

  private async Task UpdateFloorPlanItemComments(List<UpdateFloorPlanCommentCommand>? comments, Guid floorPlanItemId)
  {
    if (comments == null) return;

    foreach (var comment in comments)
    {
      var floorPlanComment = await _floorPlanCommentsRepository
          .FirstOrDefaultAsync(c => c.Id == comment.Id && c.FloorPlanItemId == floorPlanItemId);

      if (floorPlanComment is null) continue;

      floorPlanComment.UserName = comment.UserName;
      floorPlanComment.Comment = comment.Comment;
      floorPlanComment.CreationTime = DateTime.Parse(comment.Date);
    }
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

    foreach (var fileUrl in fileUrlsToDelete)
    {
      string fileName = Path.GetFileName(fileUrl);
      var fileRecord = await _floorPlanFilesRepository.FirstOrDefaultAsync(f => f.FileName == fileName);
      if (fileRecord != null)
      {
        await _floorPlanFilesRepository.DeleteAsync(fileRecord);
      }
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
