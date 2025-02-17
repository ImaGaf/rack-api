using System.Reflection;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.ContributorAggregate;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Infrastructure.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options,
  IDomainEventDispatcher? dispatcher) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
  private readonly IDomainEventDispatcher? _dispatcher = dispatcher;

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
  public DbSet<FloorPlanItem> FloorPlanItems => Set<FloorPlanItem>();
  public DbSet<FloorPlanComments> FloorPlanComments => Set<FloorPlanComments>();
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    modelBuilder.Entity<Products>()
      .HasOne(p => p.Manufacturer)
      .WithMany(b => b.Products)
      .HasForeignKey(p => p.ManufacturerId)
      .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Warehouse>()
      .HasOne(w => w.User)
      .WithMany(u => u.OwnedWarehouses)
      .HasForeignKey(w => w.OwnerId)
      .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Invitation>()
      .HasOne(w => w.User)
      .WithMany(u => u.Invitations)
      .HasForeignKey(w => w.InviteById)
      .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<UserOtp>()
        .HasOne(u => u.User)
        .WithMany(u => u.UserOtps)
        .HasForeignKey(u => u.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<FloorPlan>()
      .HasOne(f => f.Warehouse)
      .WithMany(w => w.FloorPlans)
      .HasForeignKey(f => f.WarehouseId)
      .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<FloorPlanFiles>()
       .HasOne(fp => fp.FloorPlan)
       .WithMany(f => f.FloorPlanFiles)
       .HasForeignKey(fp => fp.FloorPlanId)
       .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<WarehouseFinalUses>()
       .HasOne(wf => wf.User)
       .WithMany()
       .HasForeignKey(wf => wf.UserId)
       .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<WarehouseFinalUses>()
      .HasOne(wf => wf.Warehouse)
      .WithMany()
      .HasForeignKey(wf => wf.WarehouseId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<FloorPlanItem>()
      .HasMany(fpi => fpi.Comments)
      .WithOne(fpc => fpc.FloorPlanItem)
      .HasForeignKey(fpc => fpc.FloorPlanItemId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<FloorPlanComments>()
      .HasOne(fpc => fpc.FloorPlanItem)
      .WithMany(fpi => fpi.Comments)
      .HasForeignKey(fpc => fpc.FloorPlanItemId)
      .OnDelete(DeleteBehavior.Cascade);
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
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
