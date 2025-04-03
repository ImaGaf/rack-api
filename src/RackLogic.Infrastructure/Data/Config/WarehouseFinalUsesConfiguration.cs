using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RackLogic.Core.Common.Entity;

namespace RackLogic.Infrastructure.Data.Config;

public class WarehouseFinalUsesConfiguration : IEntityTypeConfiguration<WarehouseFinalUses>
{
  public void Configure(EntityTypeBuilder<WarehouseFinalUses> builder)
  {
    builder.HasOne(wf => wf.User).WithMany().HasForeignKey(wf => wf.UserId).OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(wf => wf.Warehouse).WithMany().HasForeignKey(wf => wf.WarehouseId).OnDelete(DeleteBehavior.Cascade);
  }
}
