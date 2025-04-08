using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Brands;

namespace RackLogic.UseCases.BayConfigurator.CreateProduct;
public class CreateCustomComponentHandler
  (IRepositoryWithExpression<Products> _productRepository)
  : ICommandHandler<CreateCustomComponentCommand, Result<ProductDTO>>
{
  public async Task<Result<ProductDTO>> Handle(CreateCustomComponentCommand request, CancellationToken cancellationToken)
  {
    var result = await _productRepository.AddAsync(request.Mapping());

    return result.ToProductDto();
  }
}
