using Ardalis.SharedKernel;

namespace RackLogic.Core.Common.Entity;
public class Warehouse : EntityBase<Guid>
{
  public required string Name { get; set; }

  public required ICollection<UserWarehouseRole> UserWarehouseRoles { get; set; }
}
