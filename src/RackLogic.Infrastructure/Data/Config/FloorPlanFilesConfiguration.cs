using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class FloorPlanFilesConfiguration : IEntityTypeConfiguration<FloorPlanFiles>
{
  public void Configure(EntityTypeBuilder<FloorPlanFiles> builder)
  {
    builder.HasOne(fp => fp.FloorPlan).WithMany(f => f.FloorPlanFiles).HasForeignKey(fp => fp.FloorPlanId).OnDelete(DeleteBehavior.Cascade);
  }
}
