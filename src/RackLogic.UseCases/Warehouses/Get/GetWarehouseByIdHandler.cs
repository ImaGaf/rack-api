using Ardalis.GuardClauses;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.Get;

public class GetWarehouseByIdHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetWarehouseByIdQuery, Result<WarehouseProfileDTO>>
{
  public async Task<Result<WarehouseProfileDTO>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
  {
    var warehouse = Guard.Against.Null(await _userWarehouse.GetWarehouseByIdAsync(request.WarehouseId));

    return Result.Success(warehouse);
  }
}
