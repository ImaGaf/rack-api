using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetListProductsBrand;
public record GetProductsByBrandQuery(
  Guid? BrandId,
  Guid? WarehouseId,
  bool? IsGetAll,
  string? Search,
  int PageNumber,
  int PageSize) : IQuery<Result<PagedDataDTO<ProductDTO>>>;
