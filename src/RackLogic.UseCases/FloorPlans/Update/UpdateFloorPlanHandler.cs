using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Update;
public class UpdateFloorPlanHandler(
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFloorPlanService _floorPlanService,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<UpdateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(UpdateFloorPlanCommand request, CancellationToken cancellationToken)
  {
    // Check if WarehouseId is valid
    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    // Check user role
    var userCreatedId = request.UserCreatedId;
    if (!await CheckPermissionUserRequest(userCreatedId, request.WarehouseId)) return Result.Forbidden();

    // Get a list of existing and non-existing FloorPlans
    var (existFloorAssetsDb, notExistFloorAssetsDb) = await _floorPlanService.GetExistingAndNewFloorPlansAsync(request);

    var validationErrors = new List<ValidationError>();

    // Handle FloorPlanAssets instances in DB
    var updateResult = await _floorPlanService.ProcessExistingFloorPlansAsync(existFloorAssetsDb, request.WarehouseId);
    if (!updateResult.IsSuccess)
    {
      validationErrors.AddRange(updateResult.ValidationErrors);
    }

    // Handle FloorPlanAssets not instances in DB
    var createResult = await _floorPlanService.CreateNewFloorPlansAsync(notExistFloorAssetsDb, request.WarehouseId, request.UserCreatedId);
    if (!createResult.IsSuccess)
    {
      validationErrors.AddRange(createResult.ValidationErrors);
    }

    return validationErrors.Any() ? Result<bool>.Invalid(validationErrors) : Result.Success(true);
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
