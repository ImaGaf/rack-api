using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.List;
public class ListBayConfiguratorHandler (IListBayConfiguratorQueryService _listBayConfiguratorQueryService)
  : IQueryHandler<ListBayConfiguratorQuery, Result<PagedDataDTO<BayConfigurationDTO>>>
{
  public async Task<Result<PagedDataDTO<BayConfigurationDTO>>> Handle(ListBayConfiguratorQuery request, CancellationToken cancellationToken)
  {
    var result = await _listBayConfiguratorQueryService.ListAsync(request);

    return Result.Success(result);
  }
}
