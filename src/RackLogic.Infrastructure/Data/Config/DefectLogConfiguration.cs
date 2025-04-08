using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class DefectLogConfiguration : IEntityTypeConfiguration<DefectLog>
{
  public void Configure(EntityTypeBuilder<DefectLog> builder)
  {
    builder.HasOne(d => d.Component).WithMany().HasForeignKey(d => d.ComponentId).OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(d => d.FloorPlanItem).WithMany(f => f.DefectLogs).HasForeignKey(d => d.FloorPlanItemId).OnDelete(DeleteBehavior.Cascade);

    builder.Property(d => d.Comment).HasMaxLength(1000);

    builder.Property(d => d.TicketId).HasMaxLength(10);
  }
}
