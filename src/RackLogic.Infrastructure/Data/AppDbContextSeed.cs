using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;

namespace RackLogic.Infrastructure.Data;
public class AppDbContextSeed
{
  public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
  {
    var roles = new[] 
    { 
      RoleConstant.Premises,
      RoleConstant.Inspector,
      RoleConstant.ClientManager,
      RoleConstant.ClientEditor,
      RoleConstant.Repairer
    };

    foreach (var role in roles)
    {
      if (!await roleManager.RoleExistsAsync(role))
      {
        await roleManager.CreateAsync(new IdentityRole<Guid>(role));
      }
    }
  }
}
