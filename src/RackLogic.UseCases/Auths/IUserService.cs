using System.Security.Claims;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Auths.Login;

namespace RackLogic.UseCases.Auths;
public interface IUserService
{
  Task<LoginDTO> GenerateUserTokenAsync(ApplicationUser user);
  Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token); 
  Task UpdateSecurityStampByUserNameAsync(string userName);
}
