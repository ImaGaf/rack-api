using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Warehouses.GetWarehouseFinalUse;
public class GetWarehouseFinalUseHandler(
  IUserWarehouseService _userWarehouse,
  IUser _user)
  : IQueryHandler<GetWarehouseFinalUseQuery, Result<WarehousesDTO>>
{
  public async Task<Result<WarehousesDTO>> Handle(GetWarehouseFinalUseQuery request, CancellationToken cancellationToken)
  {
    if (!_user.IsAuthenticated!.Value) return Result.Unauthorized();

    var userIdClaim = _user.Id;
    if (userIdClaim == null) return Result.Invalid(new ValidationError("User not found."));
    var userId = userIdClaim.Value;

    var warehouseDTO = await _userWarehouse.GetLastWarehouseForUserAsync(userId);

    if (warehouseDTO == null) return Result.Success();

    return Result.Success(warehouseDTO);
  }
}
