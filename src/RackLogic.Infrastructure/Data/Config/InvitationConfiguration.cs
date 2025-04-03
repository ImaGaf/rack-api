using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
  public void Configure(EntityTypeBuilder<Invitation> builder)
  {
    builder.HasOne(w => w.User).WithMany(u => u.Invitations).HasForeignKey(w => w.InviteById).OnDelete(DeleteBehavior.Restrict);
  }
}
