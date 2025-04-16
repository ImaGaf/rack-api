using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Warehouses.GetListWarehouses;
public class GetListWarehousesHandler(
  IUserWarehouseService _userWarehouse,
  IUser _user
  ) : IQueryHandler<GetListWarehousesQuery, Result<List<WarehousesDTO>>>
{
  public async Task<Result<List<WarehousesDTO>>> Handle(GetListWarehousesQuery request, CancellationToken cancellationToken)
  {
    var Warehouses = await _userWarehouse.GetWarehousesByUserIdAsync(_user.Id!.Value);
    if (Warehouses == null)
    {
      return Result.Success(new List<WarehousesDTO>());
    }
    return Result.Success(Warehouses);
  }
}
