using RackLogic.UseCases.Auths.Login;
using RackLogic.UseCases.Auths.RefeshToken;
using RackLogic.Web.Auth.Login;
using RackLogic.Web.Auth.RefreshToken;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Auth;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class AuthMapper
{
  public static partial LoginCommand ToLoginCommand(this LoginRequest request);
  public static partial LoginResponse ToLoginResponse(this LoginDTO request);

  public static partial RefreshTokenCommand ToRefreshTokenCommand(this RefreshTokenRequest request);
  public static partial RefreshTokenResponse ToRefreshTokenResponse(this RefreshTokenDTO request);
}
