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
                PermissionConstant.DefectLog.Create,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.AuthoriseResolve,
                PermissionConstant.Inspection.Full,
                PermissionConstant.FloorPlan.ViewFloorplan,
                PermissionConstant.BayConfiguration.ViewBayConfiguration
            }
        },
        {
            "premises", new List<string>
            {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.UserRole.Full,
                PermissionConstant.DefectLog.Create,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.AuthoriseResolve,
                PermissionConstant.FloorPlan.Full,
                PermissionConstant.BayConfiguration.Full,
            }
        },
        {
          "client-admin", new List<string>
          {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.DefectLog.Create,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.AuthoriseResolve,
                PermissionConstant.FloorPlan.Full,
                PermissionConstant.BayConfiguration.Full,
          }
        },
        {
            "client-manager", new List<string>
            {
                PermissionConstant.Account.FullThemselves,
                PermissionConstant.UserRole.CreateEditor,
                PermissionConstant.UserRole.CreateRepairer,
                PermissionConstant.UserRole.UpdateEditor,
                PermissionConstant.UserRole.UpdateRepairer,
                PermissionConstant.UserRole.DeleteEditor,
                PermissionConstant.UserRole.DeleteRepairer,
                PermissionConstant.DefectLog.Create,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.AuthoriseResolve,
                PermissionConstant.FloorPlan.ViewFloorplan,
                PermissionConstant.BayConfiguration.ViewBayConfiguration
            }
        },
        {
            "client-editor", new List<string>
            {
                PermissionConstant.DefectLog.Create,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.UpdateStatus,
                PermissionConstant.FloorPlan.UpdateFloorplan,
                PermissionConstant.FloorPlan.ViewFloorplan,
                PermissionConstant.BayConfiguration.ViewBayConfiguration,
                PermissionConstant.BayConfiguration.UpdateBayConfiguration
            }
        },
        {
            "repairer", new List<string>
            {
                PermissionConstant.Account.ViewEveryone,
                PermissionConstant.DefectLog.View,
                PermissionConstant.DefectLog.UpdateStatus,
                PermissionConstant.FloorPlan.ViewFloorplan,
                PermissionConstant.BayConfiguration.ViewBayConfiguration
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
