using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.InviteUser;
public record InviteUserCommand(Guid InviteById, string Email, Guid WarehouseId, string Role) : ICommand<Result<bool>>;
