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
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

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
