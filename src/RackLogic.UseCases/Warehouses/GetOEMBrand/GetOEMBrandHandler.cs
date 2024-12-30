using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetOEMBrand;
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
