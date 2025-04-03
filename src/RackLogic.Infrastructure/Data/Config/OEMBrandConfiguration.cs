using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class OEMBrandConfiguration : IEntityTypeConfiguration<OEMBrand>
{
  public void Configure(EntityTypeBuilder<OEMBrand> builder)
  {
    builder.HasOne(p => p.Warehouse).WithMany(u => u.OEMBrands).HasForeignKey(p => p.WarehouseId).OnDelete(DeleteBehavior.NoAction);
  }
}
