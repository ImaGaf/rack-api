using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListWarehouses;
public class GetListWarehousesHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetListWarehousesQuery, Result<List<WarehousesDTO>>>
{
  public async Task<Result<List<WarehousesDTO>>> Handle(GetListWarehousesQuery request, CancellationToken cancellationToken)
  {
    var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null) return Result.Invalid(new ValidationError("User not found."));
    var userId = Guid.Parse(userIdClaim.Value);

    var Warehouses = await _userWarehouse.GetWarehousesByUserIdAsync(userId);
    if (Warehouses == null)
    {
      return Result.Success(new List<WarehousesDTO>());
    }
    return Result.Success(Warehouses);
  }
}
