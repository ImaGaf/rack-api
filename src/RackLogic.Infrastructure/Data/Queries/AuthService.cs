using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.UserAggregate;
using RackLogic.Infrastructure.Token;
using RackLogic.UseCases.Auths;
using RackLogic.UseCases.Auths.Login;

namespace RackLogic.Infrastructure.Data.Queries;
public class AuthService(
        UserManager<ApplicationUser> userManager,
        AppDbContext applicationDbContext,
        TokenSettings tokenSettings) : IAuthService
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;
  private readonly TokenSettings _tokenSettings = tokenSettings;
  private readonly AppDbContext _context = applicationDbContext;
  public async Task<LoginDTO> GenerateUserTokenAsync(ApplicationUser user, Guid? warehouseId)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimConstant.Username, user.UserName ?? ""),
      new(ClaimTypes.Name, user.FirstName ?? ""),
      new Claim(ClaimTypes.Role, user.RegisteredRole ?? "")
    };

    var userWarehouseRole = await _context.UserWarehouseRoles
      .Include(uwr => uwr.Role)
      .FirstOrDefaultAsync(uwr => uwr.UserId == user.Id && uwr.WarehouseId == warehouseId);

    if (userWarehouseRole != null && warehouseId != null)
    {
      var roleName = userWarehouseRole.Role!.Name!;
      claims.Add(new Claim(ClaimConstant.WarehouseRole, roleName));

      var permissions = await _context.RolePermissions
        .Where(rp => rp.RoleId == userWarehouseRole.RoleId && !string.IsNullOrEmpty(rp.Code))
        .Select(rp => rp.Code!)
        .ToListAsync();

      var accessRights = JsonSerializer.Serialize(permissions);

      claims.Add(new Claim(ClaimConstant.Permissions, accessRights));
      claims.Add(new Claim(ClaimConstant.WarehouseId, userWarehouseRole.WarehouseId.ToString()));
    }

    var token = TokenUtil.GetToken(_tokenSettings, user, claims);
    await _userManager.RemoveAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
    var refreshToken = await _userManager.GenerateUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
    await _userManager.SetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", refreshToken);
    return new LoginDTO() { AccessToken = token, RefreshToken = refreshToken, ExpiresIn = DateTime.UtcNow.AddSeconds(_tokenSettings.TokenExpireSeconds) };
  }

  public async Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token)
  {
    return await Task.Run(() => TokenUtil.GetPrincipalFromExpiredToken(_tokenSettings, token));
  }

  public async Task UpdateSecurityStampByUserNameAsync(string userName)
  {
    var appUser = _context.Users.First(x => x.UserName == userName);
    if (appUser != null)
    {
      await _userManager.UpdateSecurityStampAsync(appUser);
    }
  }
}
