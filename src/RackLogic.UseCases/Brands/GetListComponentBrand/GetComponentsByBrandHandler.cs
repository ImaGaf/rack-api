using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetListComponentBrand;
public class GetComponentsByBrandHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetComponentsByBrandQuery, Result<PagedDataDTO<ListComponentBrandDTO>>>
{
  public async Task<Result<PagedDataDTO<ListComponentBrandDTO>>> Handle(GetComponentsByBrandQuery request, CancellationToken cancellationToken)
  {
    var (products, totalRecords) = await _userWarehouse.GetProductsByBrandAsync(request.BrandId, request.PageNumber, request.PageSize);

    if (products == null)
    {
      products = new List<ListComponentBrandDTO>();
    }

    var pagedData = request.ToPagedDataDTO(products, totalRecords);

    return Result.Success(pagedData);
  }
}
