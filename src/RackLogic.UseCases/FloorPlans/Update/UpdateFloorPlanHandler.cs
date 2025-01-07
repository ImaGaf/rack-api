using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Update;
public class UpdateFloorPlanHandler(
  IRepositoryWithExpression<FloorPlan> _floorPlanRepository,
  IRepositoryWithExpression<FloorPlanFiles> _floorPlanFilesRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
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

    foreach (var plan in request.FloorPlans)
    {
      var floorPlan = await _floorPlanRepository.FirstOrDefaultAsync(x => x.Id == plan.FloorPlanId);
      results.Add(floorPlan != null);
      if (floorPlan is null) continue;

      floorPlan.AssetName = plan.AssetName;
      floorPlan.Floor = plan.Floor;

      await _floorPlanRepository.UpdateAsync(floorPlan);

      if (plan.FileUrlsToDelete != null && plan.FileUrlsToDelete.Any())
      {
        await _fileService.DeleteFilesAsync(plan.FileUrlsToDelete);

        foreach (var fileUrl in plan.FileUrlsToDelete)
        {
          string fileName = Path.GetFileName(fileUrl);

          var fileRecord = await _floorPlanFilesRepository.FirstOrDefaultAsync(f => f.FileName == fileName);
          if (fileRecord != null)
          {
            await _floorPlanFilesRepository.DeleteAsync(fileRecord);
          }
        }
      }

      if (plan.FilesToUpdate != null && plan.FilesToUpdate.Any())
      {
        var updatedFiles = await _fileService.ReplaceFileAsync(request.WarehouseId, floorPlan.Id, plan.FilesToUpdate);

        foreach (var updatedFile in updatedFiles)
        {
          
          var fileRecord = await _floorPlanFilesRepository
              .FirstOrDefaultAsync(f => f.FloorPlanId == floorPlan.Id && f.FileName == updatedFile.FileName);

          if (fileRecord == null)
          {
            continue;
          }
        }
      }

      if (plan.NewFiles != null && plan.NewFiles.Any())
      {
        var updateNewFiles = await _fileService.SaveFilesAsync(request.WarehouseId, floorPlan.Id, plan.NewFiles);

        // Save file info to database
        var floorPlanFiles = updateNewFiles.Select(f => new FloorPlanFiles
        {
          Id = Guid.NewGuid(),
          FloorPlanId = floorPlan.Id,
          FileName = f.FileName,
          FileUrl = f.FilePath,
          ContentType = f.ContentType
        }).ToList();

        await _floorPlanFilesRepository.AddRangeAsync(floorPlanFiles);
      }
    }
    return !results.Any(x => x == false);
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
