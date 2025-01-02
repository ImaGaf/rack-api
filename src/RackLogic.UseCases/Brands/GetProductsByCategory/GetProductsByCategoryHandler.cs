using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetProductsByCategory;
public class GetProductsByCategoryHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetProductsByCategoryQuery, Result<List<ProductDTO>>>
{
  public async Task<Result<List<ProductDTO>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
  {
    var products = await _userWarehouse.GetProductsByCategoryAndManufacturerAsync(request.ManufacturerId, request.Category);
    if (products == null)
    {
      products = new List<ProductDTO>();
    }
    return Result.Success(products);
  }
}
