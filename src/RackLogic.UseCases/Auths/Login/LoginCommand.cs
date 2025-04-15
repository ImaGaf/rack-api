using Ardalis.Result;

namespace RackLogic.UseCases.Auths.Login;
public record LoginCommand(
  string Email,
  string Password,
  bool isRememberMe,
  Guid? WarehouseId)
  : Ardalis.SharedKernel.ICommand<Result<LoginDTO>>;
