using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListWarehouses;
public class GetListWarehousesHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetListWarehousesQuery, Result<List<WarehousesDTO>>>
{
  public async Task<Result<List<WarehousesDTO>>> Handle(GetListWarehousesQuery request, CancellationToken cancellationToken)
  {
    var Warehouses = await _userWarehouse.GetWarehousesByUserIdAsync(request.UserId);
    if (Warehouses == null)
    {
      return Result.Success(new List<WarehousesDTO>());
    }
    return Result.Success(Warehouses);
  }
}
