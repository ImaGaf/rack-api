using RackLogic.UseCases.Users;
using System.Security.Claims;
using System.Text.Json;
using RackLogic.Core.Common.Constants;

namespace RackLogic.Web.Services;

public class CurrentUser : IUser
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public CurrentUser(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Guid? Id => Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var guid)
    ? guid
    : null;
  public bool? IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated;
  public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
  public string? WarehouseRole => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimConstant.WarehouseRole);
  public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

  public IEnumerable<string>? Permissions
  {
    get
    {
      var claim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimConstant.Permissions);
      if (string.IsNullOrEmpty(claim))
        return null;

      var accessRights = JsonSerializer.Deserialize<IEnumerable<string>>(claim);
      return accessRights;
    }
  }
}
