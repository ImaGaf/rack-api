using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Products>
{
  public void Configure(EntityTypeBuilder<Products> builder)
  {
    builder.HasOne(p => p.Manufacturer).WithMany(b => b.Products).HasForeignKey(p => p.ManufacturerId).OnDelete(DeleteBehavior.Restrict);
  }
}
