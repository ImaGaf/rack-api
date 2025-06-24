using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetListProductsBrand;
public class GetProductsByBrandHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetProductsByBrandQuery, Result<PagedDataDTO<ProductDTO>>>
{
  public async Task<Result<PagedDataDTO<ProductDTO>>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
  {
    var (products, totalRecords) = await _userWarehouse.GetProductsByBrandAsync(request.BrandId,request.WarehouseId, request.PageNumber, request.PageSize, request.IsGetAll ?? false, request.Search);

    var pagedData = request.ToPagedDataDTO(products, totalRecords);

    return Result.Success(pagedData);
  }
}
