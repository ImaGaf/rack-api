using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class DefectLogFilesConfiguration : IEntityTypeConfiguration<DefectLogFiles>
{
  public void Configure(EntityTypeBuilder<DefectLogFiles> builder)
  {
    builder.HasOne(f => f.DefectLog).WithMany(d => d.DefectLogFiles).HasForeignKey(f => f.DefectLogId).OnDelete(DeleteBehavior.Cascade);

    builder.Property(f => f.FileName).HasMaxLength(100);

    builder.Property(f => f.FileUrl).HasMaxLength(500);

    builder.Property(f => f.ContentType).HasMaxLength(50);
  }
}
