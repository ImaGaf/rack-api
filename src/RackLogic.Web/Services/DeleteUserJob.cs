using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.Infrastructure.Data;

namespace RackLogic.Web.Services;

public class DeleteUserJob : BackgroundService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly string _warehouseRootPath;
  private readonly ILogger<DeleteUserJob> _logger;

  public DeleteUserJob(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DeleteUserJob> logger)
  {
    _serviceProvider = serviceProvider;
    _warehouseRootPath = configuration["WarehouseRootPath"] ?? "wwwroot/Warehouse";
    _logger = logger; 
  }
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        // Process users to be deleted
        await ProcessUsersToDeleteAsync(stoppingToken);
      }
      catch (Exception ex)
      {
        // Log errors
        Console.WriteLine($"Error in DeleteUserJob: {ex.Message}");
      }
      finally
      {
        // Wait 1 day before doing it again
        await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
      }
    }
  }

  /// <summary>
  /// Find and process users that have been marked for deletion after 14 days.
  /// </summary>
  private async Task ProcessUsersToDeleteAsync(CancellationToken cancellationToken)
  {
    using var scope = _serviceProvider.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Find users whose DeleteDate is not null and is older than 14 days
    var usersToDelete = await userManager.Users
        .Where(u => u.DeleteDate.HasValue && u.DeleteDate.Value.AddDays(14) <= DateTime.UtcNow)
        .ToListAsync(cancellationToken);

    // Handle each user
    foreach (var user in usersToDelete)
    {
      await HandleUserDeletionAsync(user, cancellationToken);
    }
  }

  /// <summary>
  /// Handles user deletion logic based on their role.
  /// </summary>
  private async Task HandleUserDeletionAsync(ApplicationUser user, CancellationToken cancellationToken)
  {
    using var scope = _serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var warehouseRepository = scope.ServiceProvider.GetRequiredService<IRepositoryWithExpression<Warehouse>>();
    var userWarehouseRoleRepository = scope.ServiceProvider.GetRequiredService<IRepositoryWithExpression<UserWarehouseRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var role = user.RegisteredRole;

    if (role == RoleConstant.Premises)
    {
      // Handling user Premises
      await HandlePremisesDeletionAsync(user, userManager, dbContext, roleManager, warehouseRepository, userWarehouseRoleRepository, cancellationToken);
    }
    else if (role == RoleConstant.Inspector)
    {
      // Handling user Inspector
      await HandleInspectorDeletionAsync(user, userManager, warehouseRepository, cancellationToken);
    }
    else
    {
      // Handling other user
      await HandleGenericUserDeletionAsync(user, userManager, userWarehouseRoleRepository, cancellationToken);
    }
  }

  /// <summary>
  /// Delete user Premises and related data.
  /// </summary>
  private async Task HandlePremisesDeletionAsync(
      ApplicationUser user,
      UserManager<ApplicationUser> userManager,
      AppDbContext dbContext,
      RoleManager<IdentityRole<Guid>> roleManager,
      IRepositoryWithExpression<Warehouse> warehouseRepository,
      IRepositoryWithExpression<UserWarehouseRole> userWarehouseRoleRepository,
      CancellationToken cancellationToken)
  {
    // Get ClientManager and ClientEditor role information
    var clientManagerRole = await roleManager.FindByNameAsync(RoleConstant.ClientManager);
    var clientEditorRole = await roleManager.FindByNameAsync(RoleConstant.ClientEditor);

    //Get list of warehouses belonging to user Premises
    var userWarehouses = await warehouseRepository.GetListAsync(w => w.OwnerId == user.Id, cancellationToken);

    //Delele folder in wwwroot related to Warehouse
    await DeleteWarehouseFoldersAsync(userWarehouses.Select(w => w.Id).ToList());

    // Get the list of Users in the warehouses of the deleted user
    var relatedUserRoles = await userWarehouseRoleRepository.GetListAsync(
        uwr => userWarehouses.Select(w => w.Id).Contains(uwr.WarehouseId) &&
               (uwr.RoleId == clientManagerRole!.Id || uwr.RoleId == clientEditorRole!.Id),
        cancellationToken);

    // Get a list of related accounts to delete( manager, editor)
    var relatedAccounts = await userManager.Users
        .Where(u => relatedUserRoles.Select(uwr => uwr.UserId).Contains(u.Id) &&
                    u.RegisteredRole != RoleConstant.Inspector &&
                    u.RegisteredRole != RoleConstant.Repairer)
        .ToListAsync(cancellationToken);

    // Delete related accounts
    var userIds = relatedAccounts.Select(u => u.Id).ToList();
    dbContext.Users.RemoveRange(dbContext.Users.Where(u => userIds.Contains(u.Id)));
    await dbContext.SaveChangesAsync(cancellationToken);

    // Delete warehouses created by the Premises account
    await warehouseRepository.DeleteRangeAsync(userWarehouses, cancellationToken);

    //Delete account Premises
    await userManager.DeleteAsync(user);

    // Delete user from other warehouses
    await DeleteUserRolesFromWarehousesAsync(user.Id, cancellationToken);
  }

  /// <summary>
  /// Delete user Inspector and related data.
  /// </summary>
  private async Task HandleInspectorDeletionAsync(
      ApplicationUser user,
      UserManager<ApplicationUser> userManager,
      IRepositoryWithExpression<Warehouse> warehouseRepository,
      CancellationToken cancellationToken)
  {
    // Delete warehouses created by Inspector account
    var userWarehouses = await warehouseRepository.GetListAsync(w => w.OwnerId == user.Id, cancellationToken);
    await warehouseRepository.DeleteRangeAsync(userWarehouses, cancellationToken);

    //Delele folder in wwwroot related to Warehouse
    await DeleteWarehouseFoldersAsync(userWarehouses.Select(w => w.Id).ToList());

    // Delete account Inspector
    await userManager.DeleteAsync(user);

    // Delete user from other warehouses
    await DeleteUserRolesFromWarehousesAsync(user.Id, cancellationToken);
  }

  /// <summary>
  /// Delete other users (ClientManager, ClientEditor, Repairer).
  /// </summary>
  private async Task HandleGenericUserDeletionAsync(
      ApplicationUser user,
      UserManager<ApplicationUser> userManager,
      IRepositoryWithExpression<UserWarehouseRole> userWarehouseRoleRepository,
      CancellationToken cancellationToken)
  {
    //  Delete user account
    await userManager.DeleteAsync(user);

    // Delete user from other warehouses
    await DeleteUserRolesFromWarehousesAsync(user.Id, cancellationToken);
  }

  /// <summary>
  /// Delete user roles in all warehouses.
  /// </summary>
  private async Task DeleteUserRolesFromWarehousesAsync(Guid userId, CancellationToken cancellationToken)
  {
    using var scope = _serviceProvider.CreateScope();
    var userWarehouseRoleRepository = scope.ServiceProvider.GetRequiredService<IRepositoryWithExpression<UserWarehouseRole>>();

    // Find and delete roles associated with userId
    var userRoles = await userWarehouseRoleRepository.GetListAsync(uwr => uwr.UserId == userId, cancellationToken);
    await userWarehouseRoleRepository.DeleteRangeAsync(userRoles, cancellationToken);
  }

  /// <summary>
  /// Delete warehouse folders from wwwroot.
  /// </summary>
  private async Task DeleteWarehouseFoldersAsync(IEnumerable<Guid> warehouseIds)
  {
    foreach (var warehouseId in warehouseIds)
    {
      var folderPath = Path.Combine(_warehouseRootPath, warehouseId.ToString());
      if (Directory.Exists(folderPath))
      {
        try
        {
          Directory.Delete(folderPath, true);
        }
        catch (Exception ex)
        {
          _logger.LogError($"Error deleting folder {folderPath}: {ex.StackTrace}");
        }
      }
    }
    await Task.CompletedTask;
  }
}
