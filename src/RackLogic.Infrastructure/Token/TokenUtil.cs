using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Infrastructure.Token;
public static class TokenUtil
{
  public static string GetToken(TokenSettings appSettings, ApplicationUser user, List<Claim> roleClaims)
  {
    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey));
    var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    var userClaims = new List<Claim>
      {
          new(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new ("UserName", user.UserName??""),
          new (ClaimTypes.Name, user.FirstName??""),
          new Claim(ClaimTypes.Role, user.RegisteredRole ?? "")
      };
    userClaims.AddRange(roleClaims);
    var tokeOptions = new JwtSecurityToken(
        issuer: appSettings.Issuer,
        audience: appSettings.Audience,
        claims: userClaims,
        expires: DateTime.UtcNow.AddSeconds(appSettings.TokenExpireSeconds),
        signingCredentials: signInCredentials
    );
    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    return tokenString;
  }

  public static ClaimsPrincipal GetPrincipalFromExpiredToken(TokenSettings appSettings, string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidAudience = appSettings.Audience,
      ValidIssuer = appSettings.Issuer,
      ValidateLifetime = false,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey))
    };

    var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
    if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
      throw new SecurityTokenException("GetPrincipalFromExpiredToken Token is not validated");

    return principal;
  }

  public static Task<T?> DecodeJwt<T>(string token)
  {
    var handler = new JwtSecurityTokenHandler();
    var jsonToken = handler.ReadJwtToken(token);
    var payload = jsonToken.Payload.SerializeToJson();
    return Task.FromResult(JsonConvert.DeserializeObject<T>(payload ?? string.Empty));
  }
}
