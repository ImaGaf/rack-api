using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetWarehouseFinalUse;
public record GetWarehouseFinalUseQuery() : IQuery<Result<WarehousesDTO>>;
