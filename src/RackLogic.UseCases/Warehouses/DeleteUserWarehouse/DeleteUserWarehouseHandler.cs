using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.Warehouses.DeleteUserWarehouse;
public class DeleteUserWarehouseHandler(
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<DeleteUserWarehouseCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(DeleteUserWarehouseCommand request, CancellationToken cancellationToken)
  {
    // Check if WarehouseId is valid
    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    // Check user permission role 
    if (!await CheckPermissionUserRequestAsync(request.UserCreationId, request.WarehouseId)) return Result.Forbidden("User does not have delete permission!");

    // Check if the selected user exists in the Warehouse
    var userWarehouseDelete = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.WarehouseId == request.WarehouseId && x.UserId == request.UserId);
    if (userWarehouseDelete == null) return Result.Invalid(new ValidationError("The selected user does not exist in this Warehouse."));

    // Delete user from Warehouse
    await _userWarehouseRoleRepository.DeleteAsync(userWarehouseDelete);

    //TODO delete account user from system( CLIENT-MANAGER, CLIENT-EDITOR)
    return Result.Success(true);
  }

  private async Task<bool> CheckPermissionUserRequestAsync(Guid userId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises) return false;

    return true;
  }
}
