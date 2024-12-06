using Ardalis.Result;

namespace RackLogic.UseCases.Auths.Login;
public record LoginCommand(
  string Email,
  string Password,
  bool isRememberMe)
  : Ardalis.SharedKernel.ICommand<Result<LoginDTO>>;
