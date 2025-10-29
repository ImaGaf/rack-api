using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
  public void Configure(EntityTypeBuilder<Warehouse> builder)
  {
    builder
      .Property(p => p.OwnerId)
      .IsRequired(false);

    builder
      .HasOne(w => w.User)
      .WithMany(u => u.OwnedWarehouses)
      .HasForeignKey(w => w.OwnerId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
