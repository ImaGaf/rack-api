using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Auths.RefeshToken;
public class RefreshTokenHandler(
  UserManager<ApplicationUser> _userManager,
  IUserWarehouseService _userWarehouseService,
  IAuthService _userService)
  : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenDTO>>
{
  public async Task<Result<RefreshTokenDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
  {
    var principal = await _userService.GetPrincipalFromExpiredTokenAsync(request.AccessToken);
    if (principal == null || principal.FindFirst("UserName")?.Value == null)
    {
      return Result.Invalid(new ValidationError("User not found."));
    }

    var user = await _userManager.FindByNameAsync(principal.FindFirst("UserName")?.Value ?? "");
    if (user == null)
    {
      return Result.Invalid(new ValidationError("User not found."));
    }

    if (!await _userManager.VerifyUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", request.RefreshToken))
    {
      return Result.Invalid(new ValidationError("Refresh token expired."));
    }

    var lastVisitedWarehouse = await _userWarehouseService.GetLastWarehouseForUserAsync(user.Id);

    var token = await _userService.GenerateUserTokenAsync(user, lastVisitedWarehouse?.WarehouseId);

    return new RefreshTokenDTO() { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken, ExpiresIn = token.ExpiresIn };
  }
}
