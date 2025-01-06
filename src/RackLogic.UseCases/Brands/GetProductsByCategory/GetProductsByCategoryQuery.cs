using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Brands.GetProductsByCategory;
public record GetProductsByCategoryQuery(Guid ManufacturerId, string Category) : IQuery<Result<List<ProductDTO>>>;
