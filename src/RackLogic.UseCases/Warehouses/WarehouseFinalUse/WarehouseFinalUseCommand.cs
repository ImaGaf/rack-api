using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.WarehouseFinalUse;
public record WarehouseFinalUseCommand(
  ClaimsPrincipal User,
  Guid WarehouseId)
  : ICommand<Result<bool>>;
