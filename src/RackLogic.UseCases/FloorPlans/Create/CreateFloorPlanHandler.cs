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
  IRepositoryWithExpression<FloorPlanItem> _floorPlanItemRepository,
  IRepositoryWithExpression<FloorPlanComments> _floorPlanCommentsRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFileService _fileService,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<CreateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateFloorPlanCommand request, CancellationToken cancellationToken)
  {

    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    var userCreatedId = request.UserCreatedId;
    if (!await CheckPermissionUserRequest(userCreatedId, request.WarehouseId)) return Result.Forbidden();

    var (floorPlans, floorPlanItems, floorPlanComments) = BuildFloorPlanData(request, userCreatedId);

    if (!await SaveFloorPlanData(floorPlans, floorPlanItems, floorPlanComments))
      return Result.Invalid(new ValidationError("Failed to create floor plans."));

    await SaveFloorPlanFiles(floorPlans, request.FloorPlans);

    return Result.Success(true);
  }

  //Funcition
  private (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) BuildFloorPlanData(CreateFloorPlanCommand request, Guid userCreatedId)
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

  private async Task<bool> SaveFloorPlanData(List<FloorPlan> floorPlans, List<FloorPlanItem> floorPlanItems, List<FloorPlanComments> floorPlanComments)
  {
    var savedFloorPlans = await _floorPlanRepository.AddRangeAsync(floorPlans);
    if (savedFloorPlans == null) return false;

    var savedFloorPlanItems = await _floorPlanItemRepository.AddRangeAsync(floorPlanItems);
    if (savedFloorPlanItems == null) return false;

    await _floorPlanCommentsRepository.AddRangeAsync(floorPlanComments);
    return true;
  }

  private async Task SaveFloorPlanFiles(List<FloorPlan> floorPlans, List<FloorPlanCommand> floorPlanRequests)
  {
    foreach (var (floorPlan, f) in floorPlans.Zip(floorPlanRequests))
    {
      if (f.Files?.Any() ?? false)
      {
        var savedFiles = await _fileService.SaveFilesAsync(floorPlan.WarehouseId, floorPlan.Id, f.Files);

        var floorPlanFiles = savedFiles
            .Select(sf => sf.ToFloorPlanFile(floorPlan.Id))
            .ToList();

        await _floorPlanFilesRepository.AddRangeAsync(floorPlanFiles);
      }
    }
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
