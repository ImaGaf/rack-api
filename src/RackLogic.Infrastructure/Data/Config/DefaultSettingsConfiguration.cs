using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class DefaultSettingsConfiguration : IEntityTypeConfiguration<DefaultSettings>
{
  public void Configure(EntityTypeBuilder<DefaultSettings> builder)
  {
    builder.HasOne(ds => ds.BayConfiguration).WithMany(ds => ds.DefaultSettings).HasForeignKey(ds => ds.BayConfigurationId).OnDelete(DeleteBehavior.Cascade);
  }
}
