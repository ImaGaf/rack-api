using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class FloorPlanCommentsConfiguration : IEntityTypeConfiguration<FloorPlanComments>
{
  public void Configure(EntityTypeBuilder<FloorPlanComments> builder)
  {
    builder.HasOne(fpc => fpc.FloorPlanItem).WithMany(fpi => fpi.Comments).HasForeignKey(fpc => fpc.FloorPlanItemId).OnDelete(DeleteBehavior.Cascade);
  }
}
