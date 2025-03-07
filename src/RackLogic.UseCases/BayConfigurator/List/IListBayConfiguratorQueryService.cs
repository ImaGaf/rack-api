using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.List;
public interface IListBayConfiguratorQueryService
{
  Task<PagedDataDTO<BayConfigurationDTO>> ListAsync(Guid warehouseId, int pageNumber, int pageSize);
}
