using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Infrastructure.Data.Config;

public class UserOtpConfiguration : IEntityTypeConfiguration<UserOtp>
{
  public void Configure(EntityTypeBuilder<UserOtp> builder)
  {
    builder.HasOne(u => u.User).WithMany(u => u.UserOtps).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade);
  }
}
