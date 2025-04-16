using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Auths;
using RackLogic.UseCases.Auths.Login;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Warehouses.WarehouseFinalUse;
public class WarehouseFinalUseHandler(
  IRepositoryWithExpression<WarehouseFinalUses> _warehouseFinalUseRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  UserManager<ApplicationUser> _userManager,
  IAuthService _authService,
  IUser _user)
  : ICommandHandler<WarehouseFinalUseCommand, Result<LoginDTO>>
{
  public async Task<Result<LoginDTO>> Handle(WarehouseFinalUseCommand request, CancellationToken cancellationToken)
  {
    if (!_user.IsAuthenticated!.Value) return Result.Unauthorized();
    var userIdClaim = _user.Id;
    if (userIdClaim == null) return Result.Invalid(new ValidationError("User not found."));
    var userId = _user.Id!.Value;

    // Check if user has permission to work in this warehouse
    var userHasAccess = await _userWarehouseRoleRepository
        .FirstOrDefaultAsync(uwr => uwr.UserId == userId && uwr.WarehouseId == request.WarehouseId, cancellationToken)
        != null;

    if (!userHasAccess) return Result.Forbidden("User does not have permission to access this warehouse.");

    // Check if the user has any record
    var existingRecord = await _warehouseFinalUseRepository
        .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

    if (existingRecord != null) await _warehouseFinalUseRepository.DeleteAsync(existingRecord, cancellationToken);

    var newWarehouseFinalUse = new WarehouseFinalUses
    {
      UserId = userId,
      WarehouseId = request.WarehouseId
    };
    await _warehouseFinalUseRepository.AddAsync(newWarehouseFinalUse, cancellationToken);

    var user = await _userManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return Result.Invalid(new ValidationError("User record not found."));

    var loginDto = await _authService.GenerateUserTokenAsync(user, request.WarehouseId);

    var result = new LoginDTO()
    {
      AccessToken = loginDto.AccessToken,
      RefreshToken = loginDto.RefreshToken,
      ExpiresIn = loginDto.ExpiresIn
    };

    return Result.Success(result);
  }
}
