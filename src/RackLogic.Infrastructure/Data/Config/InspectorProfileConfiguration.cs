using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class InspectorProfileConfiguration : IEntityTypeConfiguration<InspectorProfile>
{
  public void Configure(EntityTypeBuilder<InspectorProfile> builder)
  {
    builder.HasOne(i => i.User).WithOne(u => u.InspectorProfile).HasForeignKey<InspectorProfile>(i => i.UserId);
  }
}
