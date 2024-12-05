using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.InviteUser;
public record InviteUserCommand(
  ClaimsPrincipal User,
  string Email,
  Guid WarehouseId,
  string Role)
  : ICommand<Result<bool>>;
