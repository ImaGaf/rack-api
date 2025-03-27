using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetOEMBrand;
public class GetOEMBrandHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetOEMBrandQuery, Result<List<OEMBrandDTO>>>
{
  public async Task<Result<List<OEMBrandDTO>>> Handle(GetOEMBrandQuery request, CancellationToken cancellationToken)
  {
    var brands = await _userWarehouse.GetAllBrandsAsync();

    if (brands == null)
    {
      brands = new List<OEMBrandDTO>();
    }

    return Result.Success(brands);
  }
}
