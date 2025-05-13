using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class DefectLogConfiguration : IEntityTypeConfiguration<DefectLog>
{
  public void Configure(EntityTypeBuilder<DefectLog> builder)
  {
    builder.ToTable("DefectLogs");

    builder.HasOne(d => d.Component).WithMany().HasForeignKey(d => d.ComponentId).OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(d => d.FloorPlanItem).WithMany(f => f.DefectLogs).HasForeignKey(d => d.FloorPlanItemId).OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(d => d.Inspection).WithMany(f => f.DefectLogs).HasForeignKey(d => d.InspectionId).OnDelete(DeleteBehavior.NoAction);

    builder.Property(d => d.Comment).HasMaxLength(2000);

    builder.Property(d => d.TicketId).HasMaxLength(12);
  }
}
