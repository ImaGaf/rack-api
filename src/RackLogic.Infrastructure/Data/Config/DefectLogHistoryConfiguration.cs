using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;
public class DefectLogHistoryConfiguration : IEntityTypeConfiguration<DefectLogHistory>
{
  public void Configure(EntityTypeBuilder<DefectLogHistory> builder)
  {
    builder.HasOne(d => d.DefectLog).WithMany(dh => dh.DefectLogHistories).HasForeignKey(d => d.DefectLogId).OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Cascade);

    builder.Property(d => d.Note).HasMaxLength(1000);
  }
}
