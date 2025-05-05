using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
  public void Configure(EntityTypeBuilder<Inspection> builder)
  {
    builder.ToTable("Inspections");

    builder.HasOne(x => x.FloorPlan).WithMany(x => x.Inspections).HasForeignKey(x => x.FloorPlanId).OnDelete(DeleteBehavior.NoAction);
  }
}
