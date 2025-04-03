using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class BayConfigurationConfiguration : IEntityTypeConfiguration<BayConfiguration>
{
  public void Configure(EntityTypeBuilder<BayConfiguration> builder)
  {
    builder.HasOne(p => p.Warehouses).WithMany(u => u.Configurations).HasForeignKey(p => p.WarehouseId).OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(p => p.Manufacturer).WithMany(u => u.BayConfigurations).HasForeignKey(p => p.ManufacturerId).OnDelete(DeleteBehavior.Cascade);
  }
}
