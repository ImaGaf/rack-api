using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetAll;
public class GetAllBrandHandler(IUserWarehouseService _userWarehouseService) : IQueryHandler<GetAllBrandQuery, Result<List<GetAllBrandDTO>>>
{
  public async Task<Result<List<GetAllBrandDTO>>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
  {
    var result = await _userWarehouseService.GetAllBrandAsync();

    return Result.Success(result);
  }
}
