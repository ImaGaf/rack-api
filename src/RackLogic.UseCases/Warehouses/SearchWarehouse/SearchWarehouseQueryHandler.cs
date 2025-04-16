using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Warehouses.SearchWarehouse;

public class SearchWarehouseQueryHandler(
  IUserWarehouseService userWarehouseService,
  IUser user)
  : IQueryHandler<SearchWarehouseQuery, Result<PagedDataDTO<WarehouseDto>>>
{
  public async Task<Result<PagedDataDTO<WarehouseDto>>> Handle(SearchWarehouseQuery request, CancellationToken cancellationToken)
  {
    var userId = user.Id!.Value;
    var warehouses = await userWarehouseService.SearchWarehouseByUserIdAsync(request, userId, cancellationToken);

    return Result.Success(warehouses);
  }
}
