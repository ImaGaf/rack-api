using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
using RackLogic.Core.UserAggregate;
using RackLogic.Infrastructure.Token;
using RackLogic.UseCases.Auths;
using RackLogic.UseCases.Auths.Login;

namespace RackLogic.Infrastructure.Data.Queries;
public class UserSevice(
        UserManager<ApplicationUser> userManager,
        AppDbContext applicationDbContext,
        TokenSettings tokenSettings) : IUserService
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;
  private readonly TokenSettings _tokenSettings = tokenSettings;
  private readonly AppDbContext _context = applicationDbContext;
  public async Task<LoginDTO> GenerateUserTokenAsync(ApplicationUser user)
  {
    var claims = (from ur in _context.UserRoles
                  where ur.UserId == user.Id
                  join r in _context.Roles on ur.RoleId equals r.Id
                  join rc in _context.RoleClaims on r.Id equals rc.RoleId
                  select rc)
            .Where(rc => !string.IsNullOrEmpty(rc.ClaimValue) && !string.IsNullOrEmpty(rc.ClaimType))
            .Select(rc => new Claim(rc.ClaimType!, rc.ClaimValue!))
            .Distinct()
            .ToList();

    var roleClaims = (from ur in _context.UserRoles
                      where ur.UserId == user.Id
                      join r in _context.Roles on ur.RoleId equals r.Id
                      select r)
      .Where(r => !string.IsNullOrEmpty(r.Name))
      .Select(r => new Claim(ClaimTypes.Role, r.Name!))
      .Distinct()
      .ToList();

    claims.AddRange(roleClaims);

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
