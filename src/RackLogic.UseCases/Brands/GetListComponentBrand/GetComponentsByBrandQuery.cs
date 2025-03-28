using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetListComponentBrand;
public record GetComponentsByBrandQuery(
  Guid? BrandId,
  int PageNumber,
  int PageSize,
  bool? IsGetAll) : IQuery<Result<PagedDataDTO<ListComponentBrandDTO>>>;
