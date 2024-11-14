using Ardalis.Result;

namespace RackLogic.UseCases.Auths.RefeshToken;
public record RefreshTokenCommand (string AccessToken, string RefreshToken) : Ardalis.SharedKernel.ICommand<Result<RefreshTokenDTO>>;
