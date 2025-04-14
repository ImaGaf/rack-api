using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;
public class SWLSignageConfiguration : IEntityTypeConfiguration<SWLSignage>
{
  public void Configure(EntityTypeBuilder<SWLSignage> builder)
  {
    builder.HasOne(p => p.BayConfiguration)
      .WithOne(u => u.SWLSignage)
      .HasForeignKey<SWLSignage>(i => i.BayConfigurationId);
  }
}
