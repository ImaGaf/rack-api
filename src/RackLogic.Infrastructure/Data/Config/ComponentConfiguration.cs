using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
  public void Configure(EntityTypeBuilder<Component> builder)
  {
    builder.HasOne(p => p.BayConfiguration).WithMany(u => u.Components).HasForeignKey(p => p.BayId).OnDelete(DeleteBehavior.Cascade);
  }
}
