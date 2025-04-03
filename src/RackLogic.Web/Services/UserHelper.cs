using System.Security.Claims;

namespace RackLogic.Web.Services;

public static class UserHelper
{
  public static Guid GetUserId(this ClaimsPrincipal? user)
  {
    return Guid.Parse(user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
  }

  public static string GetUserName(this ClaimsPrincipal? user)
  {
    return user?.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? string.Empty;
  }

  public static string GetUserRegisteredRole(this ClaimsPrincipal? user)
  {
    return user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;
  }
}
