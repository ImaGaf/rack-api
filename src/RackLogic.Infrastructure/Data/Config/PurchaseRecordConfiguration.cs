using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class PurchaseRecordConfiguration : IEntityTypeConfiguration<PurchaseRecord>
{
  public void Configure(EntityTypeBuilder<PurchaseRecord> builder)
  {
    builder.HasOne(p => p.User)
      .WithOne(u => u.Purchase)
      .HasForeignKey<PurchaseRecord>(i => i.UserId);
  }
}
