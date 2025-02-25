using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

  public static string GenerateDeveloperIOSToken(string keyId, string issuerId, string bundleId)
  {
    var privateKey = @"-----BEGIN PRIVATE KEY-----
                    MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgA/9wtjQCv7qJjRmU
                    MMX0rubs75H8iJXZwsccxavKDUGgCgYIKoZIzj0DAQehRANCAAQOVZ/b7wvAIAhp
                    W5yUiiELf5k9oye0dh8QtF6iNENkL7pN8CCuJIctgitMHYgA2nAlv1X5Fo9mv+xe
                    yr2C1w4e
                    -----END PRIVATE KEY-----";
    var securityKey = new ECDsaSecurityKey(ECDsa.Create());

    // Load Private Key (PEM format)
    securityKey.ECDsa.ImportFromPem(privateKey.ToCharArray());

    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Issuer = issuerId,
      Audience = "appstoreconnect-v1",
      Claims = new Dictionary<string, object> { { "bid", bundleId } },
      Expires = DateTime.UtcNow.AddMinutes(30),
      SigningCredentials = credentials
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

    securityToken.Header["kid"] = keyId;

    return tokenHandler.WriteToken(securityToken);
  }
}
