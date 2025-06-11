using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.List;
public interface IListBayConfiguratorQueryService
{
  Task<PagedDataDTO<BayConfigurationDTO>> ListAsync(ListBayConfiguratorQuery request);
}
