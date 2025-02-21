using System.Security.Claims;

namespace RackLogic.Web.Services;

public class UserHelper
{
  public static Guid GetUserCreatedId(IHttpContextAccessor httpContextAccessor)
  {
    var user = httpContextAccessor.HttpContext?.User;
    return Guid.Parse(user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
  }

  public static string GetUserName(IHttpContextAccessor httpContextAccessor)
  {
    var user = httpContextAccessor.HttpContext?.User;
    return user?.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? string.Empty;
  }
}
