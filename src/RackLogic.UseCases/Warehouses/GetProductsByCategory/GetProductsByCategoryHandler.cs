using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetProductsByCategory;
public class GetProductsByCategoryHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetProductsByCategoryQuery, Result<List<ProductDTO>>>
{
  public async Task<Result<List<ProductDTO>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
  {
    var products = await _userWarehouse.GetProductsByCategoryAndManufacturerAsync(request.ManufacturerId, request.Category);
    if (products == null)
    {
      return Result.NotFound("No products found for the given ManufacturerID and Category.");
    }
    return Result.Success(products);
  }
}
