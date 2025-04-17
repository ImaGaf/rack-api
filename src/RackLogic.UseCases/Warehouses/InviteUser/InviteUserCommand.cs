using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.InviteUser;
public record InviteUserCommand(
  string Email,
  Guid WarehouseId,
  string Role)
  : ICommand<Result<bool>>;
