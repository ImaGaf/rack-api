using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Auths.Login;

namespace RackLogic.UseCases.Warehouses.WarehouseFinalUse;
public record WarehouseFinalUseCommand(Guid WarehouseId) : ICommand<Result<LoginDTO>>;
