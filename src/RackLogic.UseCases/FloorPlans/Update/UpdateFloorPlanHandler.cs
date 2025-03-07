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

    var validationErrors = new List<ValidationError>();

    // Use CreateOrUpdateAsync to process both existing and non-existing floor plans
    var createOrUpdateResult = await _floorPlanService.CreateOrUpdateFloorAsssetsAsync(
        request.FloorAssets,
        request.WarehouseId,
        request.UserCreatedId
    );

    if (!createOrUpdateResult.IsSuccess)
    {
      validationErrors.AddRange(createOrUpdateResult.ValidationErrors);
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
