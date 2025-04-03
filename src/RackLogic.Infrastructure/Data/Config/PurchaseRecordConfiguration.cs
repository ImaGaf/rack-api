using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class PurchaseRecordConfiguration : IEntityTypeConfiguration<PurchaseRecord>
{
  public void Configure(EntityTypeBuilder<PurchaseRecord> builder)
  {
    builder.HasOne(p => p.User).WithMany(u => u.Purchases).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
  }
}
