using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.List;
public record ListBayConfiguratorQuery(
  Guid WarehouseId,
  int PageNumber,
  int PageSize,
  bool? IsGetAll)
:IQuery<Result<PagedDataDTO<BayConfigurationDTO>>>;
