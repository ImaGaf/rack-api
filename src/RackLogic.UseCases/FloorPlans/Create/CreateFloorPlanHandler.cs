using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Create;
public class CreateFloorPlanHandler(
 // IRepositoryWithExpression<FloorPlan> _floorPlanRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFloorPlanService _floorPlanService,
RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<CreateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateFloorPlanCommand request, CancellationToken cancellationToken)
  {
    // Check validity of WarehouseId
    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    // Check if the warehouse has a floor plan
    //if (await _floorPlanRepository.FirstOrDefaultAsync(x => x.WarehouseId == request.WarehouseId) != null) return Result.Invalid(new ValidationError("This Warehouse already has a Floor Plan!"));

    // Check user permissions
    var userCreatedId = request.UserCreatedId;
    if (!await CheckPermissionUserRequest(userCreatedId, request.WarehouseId)) return Result.Forbidden();

    // Get floor plan data
    var (floorPlans, floorPlanItems, floorPlanComments) = _floorPlanService.GetFloorPlanData(request, request.UserCreatedId);

    // Save FloorPlan Data
    return await _floorPlanService.SaveFloorPlanDataAsync(floorPlans, floorPlanItems, floorPlanComments, request.FloorPlans);
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
