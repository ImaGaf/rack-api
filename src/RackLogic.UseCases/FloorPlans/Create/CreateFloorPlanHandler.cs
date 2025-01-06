using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Create;
public class CreateFloorPlanHandler(
  IRepositoryWithExpression<FloorPlan> _floorPlanRepository,
  IRepositoryWithExpression<FloorPlanFiles> _floorPlanFilesRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFileService _fileService,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<CreateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateFloorPlanCommand request, CancellationToken cancellationToken)
  {
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();

    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    var inviteIdString = request.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var userCreatedId = Guid.Parse(inviteIdString);
    if (!await CheckPermissionUserRequest(userCreatedId, request.WarehouseId)) return Result.Forbidden();

    var listFloorPlans = new List<FloorPlan>();

    foreach (var f in request.FloorPlans)
    {
      listFloorPlans.Add(new FloorPlan()
      {
        WarehouseId = request.WarehouseId,
        AssetName = f.AssetName,
        Floor = f.Floor,
        CreatorId = userCreatedId,
        FloorPlanFiles = new List<FloorPlanFiles>()
      });
    }

    var result = await _floorPlanRepository.AddRangeAsync(listFloorPlans);
    if (result == null)
    {
      return Result.Invalid(new ValidationError("Failed to create floor plans."));
    }

    //Save files
    foreach (var (floorPlan, f) in result.Zip(request.FloorPlans))
    {
      // Check if user has uploaded files
      if (f.Files != null && f.Files.Any()  )
      {
        var savedFiles = await _fileService.SaveFilesAsync(floorPlan.WarehouseId, floorPlan.Id, f.Files);

        // Save file info to database
        var floorPlanFiles = savedFiles.Select(sf => new FloorPlanFiles
        {
          Id = Guid.NewGuid(),
          FloorPlanId = floorPlan.Id,
          FileName = sf.FileName,
          FileUrl = sf.FilePath,
          ContentType = sf.ContentType
        }).ToList();

        await _floorPlanFilesRepository.AddRangeAsync(floorPlanFiles);
      }
    }

    return Result.Success(true);
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
