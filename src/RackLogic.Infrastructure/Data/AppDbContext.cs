using System.Reflection;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.ContributorAggregate;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Users;

namespace RackLogic.Infrastructure.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options, IUser user,
  IDomainEventDispatcher? dispatcher) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
  private readonly IDomainEventDispatcher? _dispatcher = dispatcher;
  private readonly IUser _user = user;

  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<UserOtp> UserOtps => Set<UserOtp>();
  public DbSet<Warehouse> Warehouses => Set<Warehouse>();
  public DbSet<UserWarehouseRole> UserWarehouseRoles => Set<UserWarehouseRole>();
  public DbSet<Invitation> Invitations => Set<Invitation>();
  public DbSet<FloorPlan> FloorPlans => Set<FloorPlan>();
  public DbSet<FloorPlanFiles> FloorPlanFiles => Set<FloorPlanFiles>();
  public DbSet<Products> Products => Set<Products>();
  public DbSet<OEMBrand> OEMBrands => Set<OEMBrand>();
  public DbSet<WarehouseFinalUses> WarehouseFinalUses => Set<WarehouseFinalUses>();
  public DbSet<PurchaseRecord> PurchaseRecords => Set<PurchaseRecord>();
  public DbSet<FloorPlanItem> FloorPlanItems => Set<FloorPlanItem>();
  public DbSet<FloorPlanComments> FloorPlanComments => Set<FloorPlanComments>();
  public DbSet<BayConfiguration> BayConfigurations => Set<BayConfiguration>();
  public DbSet<Component> Components => Set<Component>();
  public DbSet<BayConfigurationLevels> BayConfigurationLevels => Set<BayConfigurationLevels>();
  public DbSet<DefaultSettings> DefaultSettings => Set<DefaultSettings>();
  public DbSet<InspectorProfile> InspectorProfiles => Set<InspectorProfile>();
  public DbSet<Inspection> Inspections => Set<Inspection>();
  public DbSet<DefectLog> DefectLogs => Set<DefectLog>();
  public DbSet<DefectLogHistory> DefectLogHistories => Set<DefectLogHistory>();
  public DbSet<SWLSignage> SWLSignageEntries => Set<SWLSignage>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    foreach (var entry in ChangeTracker.Entries<BaseEntityCustom>())
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.CreatorId = _user?.Id;
        entry.Entity.CreationTime = DateTime.UtcNow;
      }

      if (entry.State == EntityState.Modified)
      {
        entry.Entity.LastModifierId = _user?.Id;
        entry.Entity.LastModificationTime = DateTime.UtcNow;
      }
    }

    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
