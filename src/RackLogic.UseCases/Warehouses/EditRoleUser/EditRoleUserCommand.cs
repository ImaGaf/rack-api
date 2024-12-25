using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.EditRoleUser;
public record EditRoleUserCommand(
  ClaimsPrincipal User,
  Guid UserId,
  Guid WarehouseId,
  string Role)
  : ICommand<Result<bool>>;
