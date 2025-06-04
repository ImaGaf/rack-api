using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data;

public class AppDbContextSeed
{
  public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context)
  {
    var rolePermissions = new Dictionary<string, List<string>>
    {
        {
            "inspector", new List<string>
            {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.DefectLog.Full,
                PermissionConstant.Inspection.Full,
                PermissionConstant.FloorPlan.Full,
                PermissionConstant.BayConfiguration.Full
            }
        },
        {
            "premises", new List<string>
            {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.UserRole.Full,
                PermissionConstant.DefectLog.Full,
                PermissionConstant.Inspection.ViewReport,
                PermissionConstant.Inspection.ViewSchedule,
                PermissionConstant.UserRole.Full,
                PermissionConstant.FloorPlan.Full,
                PermissionConstant.BayConfiguration.Full,
                PermissionConstant.FloorPlan.View
            }
        },
        {
            "manager", new List<string>
            {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.UserRole.CreateEditor,
                PermissionConstant.UserRole.CreateInspector,
                PermissionConstant.UserRole.CreateRepairer,
                PermissionConstant.UserRole.UpdateInspector,
                PermissionConstant.UserRole.UpdateRepairer,
                PermissionConstant.UserRole.UpdateEditor,
                PermissionConstant.UserRole.DeleteInspector,
                PermissionConstant.UserRole.DeleteRepairer,
                PermissionConstant.UserRole.DeleteRepairer,
                PermissionConstant.DefectLog.Full,
                PermissionConstant.FloorPlan.View,
                PermissionConstant.BayConfiguration.Full,
                PermissionConstant.Inspection.ViewReport,
                PermissionConstant.Inspection.ViewSchedule
            }
        },
        {
            "editor", new List<string>
            {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.DefectLog.Create,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.UpdateStatus,
                PermissionConstant.FloorPlan.View,
                PermissionConstant.BayConfiguration.View,
                PermissionConstant.Inspection.ViewReport,
                PermissionConstant.Inspection.ViewSchedule
            }
        },
        {
            "repairer", new List<string>
            {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.UpdateStatus,
                PermissionConstant.FloorPlan.View,
                PermissionConstant.BayConfiguration.View,
                PermissionConstant.Inspection.ViewReport,
                PermissionConstant.Inspection.ViewSchedule
            }
        }
    };

    foreach (var roleName in rolePermissions.Keys)
    {
      var role = await roleManager.FindByNameAsync(roleName);
      if (role == null)
      {
        role = new IdentityRole<Guid>(roleName);
        await roleManager.CreateAsync(role);
      }

      var existingPermissions = context.RolePermissions
          .Where(p => p.RoleId == role.Id)
          .Select(p => p.Code)
          .ToHashSet();

      foreach (var permissionCode in rolePermissions[roleName])
      {
        if (!existingPermissions.Contains(permissionCode))
        {
          context.RolePermissions.Add(new RolePermission
          {
            RoleId = role.Id,
            Code = permissionCode
          });
        }
      }
    }

    await context.SaveChangesAsync();
  }
}
