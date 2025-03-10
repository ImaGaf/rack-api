using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Warehouses.EditRoleUser;
public class EditRoleUserHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  UserManager<ApplicationUser> _userManager,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<EditRoleUserCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(EditRoleUserCommand request, CancellationToken cancellationToken)
  {
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();

    var UserEditIdString = request.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var UserEditId = Guid.Parse(UserEditIdString);

    if (!await CheckPermissionUserRequestAsync(UserEditId, request.WarehouseId)) return Result.Forbidden("User does not have editing permission!");

    // Check role request in database
    var role = await _roleManager.FindByNameAsync(request.Role);
    if (role is null) return Result.Invalid(new ValidationError("Role invalid."));

    // Check warehouse request in database.
    var result = await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId);
    if (result is null) return Result.Invalid(new ValidationError("Warehouse not found."));

    // Check user request in database
    var userID = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.WarehouseId == request.WarehouseId);
    if (userID is null) return Result.Invalid(new ValidationError("User not found in this WareHouse"));

    //Check if the passed user is valid
    var userBeingEdited = await _userManager.FindByIdAsync(request.UserId.ToString());
    if (userBeingEdited == null) return Result.Invalid(new ValidationError("User not found"));

    var registeredRole = userBeingEdited.RegisteredRole;

    // Check if role want to update is valid 
    if (request.Role == RoleConstant.Premises || request.Role == RoleConstant.Inspector)
    {
      if (registeredRole != request.Role)
      {
        return Result.Forbidden("Cannot update to a role different from the registered role.");
      }
    }

    // Update Role User
    userID.RoleId = role.Id;
    await _userWarehouseRoleRepository.UpdateAsync(userID, cancellationToken);
    return true;
  }

  private async Task<bool> CheckPermissionUserRequestAsync(Guid userId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises && role.Name != RoleConstant.Inspector) return false;

    return true;
  }
}
