using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Warehouses.SearchWarehouseCertificationInspection;

public class SearchWarehouseCertificationInspectionQueryHandler(
  IUserWarehouseService userWarehouseService,
  IUser user)
  : IQueryHandler<SearchWarehouseCertificationInspectionQuery, Result<PagedDataDTO<WarehouseCertificationInspectionDto>>>
{
  public async Task<Result<PagedDataDTO<WarehouseCertificationInspectionDto>>> Handle(SearchWarehouseCertificationInspectionQuery request, CancellationToken cancellationToken)
  {
    if (user.Role != "inspector")
    {
      return Result.Forbidden("You are not an inspector");
    }

    var userId = user.Id!.Value;

    var warehouses = await userWarehouseService.SearchWarehouseCertificationInspectionAsync(request, userId, cancellationToken);

    return Result.Success(warehouses);
  }
}
