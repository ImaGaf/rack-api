using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.Brands.CreateProduct;
public class CreateProductHandler
  (IRepositoryWithExpression<Products> _productRepository)
  : ICommandHandler<CreateProductCommand, Result<ProductDTO>>
{
  public async Task<Result<ProductDTO>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
  {
    var result = await _productRepository.AddAsync(request.Mapping());

    return result.Mapping();
  }
}
