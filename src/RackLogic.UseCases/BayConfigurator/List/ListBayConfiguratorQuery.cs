using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.List;
public record ListBayConfiguratorQuery(
  Guid warehouseId,
  int pageNumber,
  int pageSize)
:IQuery<Result<PagedDataDTO<BayConfigurationDTO>>>;
