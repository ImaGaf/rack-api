using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.DeleteUserWarehouse;
public record DeleteUserWarehouseCommand(
  Guid UserCreationId,
  Guid UserId,
  Guid WarehouseId)
  : ICommand<Result<bool>>;
