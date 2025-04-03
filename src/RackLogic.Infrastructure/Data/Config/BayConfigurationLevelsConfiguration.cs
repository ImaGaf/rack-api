using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class BayConfigurationLevelsConfiguration : IEntityTypeConfiguration<BayConfigurationLevels>
{
  public void Configure(EntityTypeBuilder<BayConfigurationLevels> builder)
  {
    builder.HasOne(p => p.BayConfiguration).WithMany(u => u.BayConfigurationLevels).HasForeignKey(p => p.BayId).OnDelete(DeleteBehavior.Cascade);
  }
}
