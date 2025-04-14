using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
  public void Configure(EntityTypeBuilder<Inspection> builder)
  {
    builder.ToTable("Inspections");

    builder.HasOne(x => x.FloorPlanItem).WithOne(x => x.Inspection).HasForeignKey<Inspection>(x => x.FloorPlanItemId);
  }
}
