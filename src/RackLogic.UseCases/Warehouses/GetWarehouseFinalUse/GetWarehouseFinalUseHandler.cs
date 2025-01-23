using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetWarehouseFinalUse;
public class GetWarehouseFinalUseHandler(IUserWarehouseService _userWarehouse)
  : IQueryHandler<GetWarehouseFinalUseQuery, Result<WarehousesDTO>>
{
  public async Task<Result<WarehousesDTO>> Handle(GetWarehouseFinalUseQuery request, CancellationToken cancellationToken)
  {
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();

    var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null) return Result.Invalid(new ValidationError("User not found."));
    var userId = Guid.Parse(userIdClaim.Value);

    var warehouseDTO = await _userWarehouse.GetLastWarehouseForUserAsync(userId);

    if (warehouseDTO == null)
    {
       warehouseDTO = new WarehousesDTO();
    }

    return Result.Success(warehouseDTO);
  }
}
