using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class FloorPlanConfiguration : IEntityTypeConfiguration<FloorPlan>
{
  public void Configure(EntityTypeBuilder<FloorPlan> builder)
  {
    builder.HasOne(f => f.Warehouse).WithMany(w => w.FloorPlans).HasForeignKey(f => f.WarehouseId).OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(f => f.DefaultSettings).WithOne(ds => ds.FloorPlan).HasForeignKey(ds => ds.FloorPlanId).OnDelete(DeleteBehavior.Cascade);
  }
}
