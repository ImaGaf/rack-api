using Ardalis.Result;

namespace RackLogic.UseCases.Auths.RefeshToken;
public record RefreshTokenCommand (
  string AccessToken,
  string RefreshToken,
  Guid? warehouseId)
  : Ardalis.SharedKernel.ICommand<Result<RefreshTokenDTO>>;
