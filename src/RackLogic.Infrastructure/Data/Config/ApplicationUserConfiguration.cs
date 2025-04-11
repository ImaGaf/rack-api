using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Infrastructure.Data.Config;
internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
  public void Configure(EntityTypeBuilder<ApplicationUser> builder)
  {
    builder.HasOne(u => u.Parent)
        .WithMany(u => u.Children)
        .HasForeignKey(u => u.ParentId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
