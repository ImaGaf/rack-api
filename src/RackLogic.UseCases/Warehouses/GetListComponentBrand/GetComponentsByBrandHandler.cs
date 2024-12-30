using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListComponentBrand;
public class GetComponentsByBrandHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetComponentsByBrandQuery, Result<PagedDataDTO<ListComponentBrandDTO>>>
{
  public async Task<Result<PagedDataDTO<ListComponentBrandDTO>>> Handle(GetComponentsByBrandQuery request, CancellationToken cancellationToken)
  {
    var (products, totalRecords) = await _userWarehouse.GetProductsByBrandAsync(request.BrandId, request.PageNumber, request.PageSize);

    if (products == null)
    {
      products = new List<ListComponentBrandDTO>();
    }

    var pagedData = new PagedDataDTO<ListComponentBrandDTO>
    {
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      TotalRecords = totalRecords,
      DataDTO = products
    };

    return Result.Success(pagedData);
  }
}
