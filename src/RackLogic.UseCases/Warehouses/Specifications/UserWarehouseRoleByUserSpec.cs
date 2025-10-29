using Ardalis.Specification;
using RackLogic.Core.Common.Entity;

namespace RackLogic.UseCases.Warehouses.Specifications;

public class UserWarehouseRoleByUserSpec : Specification<UserWarehouseRole>
{
  public UserWarehouseRoleByUserSpec(Guid userId)
  {
    Query
      .Where(x => x.UserId == userId)
      .AsNoTracking();
  }
}
