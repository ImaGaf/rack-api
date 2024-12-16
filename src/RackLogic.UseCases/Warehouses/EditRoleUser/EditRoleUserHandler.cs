using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.Warehouses.EditRoleUser;
public class EditRoleUserHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<EditRoleUserCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(EditRoleUserCommand request, CancellationToken cancellationToken)
  {
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();

    var inviteIdString = request.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var inviteId = Guid.Parse(inviteIdString);

    if (!await CheckPermissionUserRequest(inviteId, request.WarehouseId)) return Result.Forbidden();

    // Check role in database
    var role = await _roleManager.FindByNameAsync(request.Role);
    if (role is null) return Result.Invalid(new ValidationError("Role invalid."));

    // Check warehouse in database
    var result = await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId && x.OwnerId == inviteId);
    if (result is null) return Result.Invalid(new ValidationError("Warehouse not found."));

    ////Check list user
    //var userString = request.UserId.ToString();
    //var user = await _userManager.FindByIdAsync(userString);
    //if (user is null) return Result.Invalid(new ValidationError("User not found"));

    var userID = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.WarehouseId == request.WarehouseId);
    if (userID is null) return Result.Invalid(new ValidationError("User not found"));

    if (request.Role == RoleConstant.Premises || request.Role == RoleConstant.Inspector)
    {
      return Result.Invalid(new ValidationError("Cannot assign Premises or Inspector role."));
    }

    // TODO: Update Role User
    userID.RoleId = role.Id;
    await _userWarehouseRoleRepository.UpdateAsync(userID, cancellationToken);
    return true;
  }

  private async Task<bool> CheckPermissionUserRequest(Guid userId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises && role.Name != RoleConstant.Inspector) return false;

    return true;
  }
}
