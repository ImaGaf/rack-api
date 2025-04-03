using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class FloorPlanItemConfiguration : IEntityTypeConfiguration<FloorPlanItem>
{
  public void Configure(EntityTypeBuilder<FloorPlanItem> builder)
  {
    builder.HasMany(fpi => fpi.Comments).WithOne(fpc => fpc.FloorPlanItem).HasForeignKey(fpc => fpc.FloorPlanItemId).OnDelete(DeleteBehavior.Cascade);
  }
}
