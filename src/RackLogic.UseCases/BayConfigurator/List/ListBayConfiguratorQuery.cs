using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.List;
public record ListBayConfiguratorQuery(
  Guid WarehouseId,
  int PageNumber,
  int PageSize,
  bool? IsGetAll,
  Guid? AssetId,
  int? RackNumber)
: IQuery<Result<PagedDataDTO<BayConfigurationDTO>>>;
