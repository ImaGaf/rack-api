using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using RackLogic.Core.UserAggregate;
using RackLogic.Infrastructure.Configuration;

namespace RackLogic.Infrastructure.Token;
public static class TokenUtil
{
  public static string GetToken(TokenSettings appSettings, ApplicationUser user, List<Claim> userClaims)
  {
    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey));
    var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    var tokenOptions = new JwtSecurityToken(
        issuer: appSettings.Issuer,
        audience: appSettings.Audience,
        claims: userClaims,
        expires: DateTime.UtcNow.AddSeconds(appSettings.TokenExpireSeconds),
        signingCredentials: signInCredentials
    );
    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
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

  public static string GenerateDeveloperIOSToken(AppStoreConfig config)
  {
    var ecKey = LoadECDsaFromPem(config.PrivateKey);
    var securityKey = new ECDsaSecurityKey(ecKey);

    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Issuer = config.IssuerId,
      Audience = config.Audience,
      Claims = new Dictionary<string, object> { { "bid", config.BundleId } },
      Expires = DateTime.UtcNow.AddMinutes(3),
      SigningCredentials = credentials
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

    securityToken.Header["kid"] = config.KeyId;

    return tokenHandler.WriteToken(securityToken);
  }

  public static async Task<string> GenerateDeveloperGooglePlayToken(GooglePlayConfig config)
  {
    string jwt = CreateJwt(config.ClientEmail, config.PrivateKey);
    return await GetAccessToken(jwt);
  }

  static string CreateJwt(string clientEmail, string privateKey)
  {
    var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    var claims = new List<Claim>
          {
              new Claim("iss", clientEmail),
              new Claim("scope", "https://www.googleapis.com/auth/androidpublisher"),
              new Claim("aud", "https://oauth2.googleapis.com/token"),
              new Claim("exp", (now + 180).ToString(), ClaimValueTypes.Integer64),
              new Claim("iat", now.ToString(), ClaimValueTypes.Integer64)
          };

    var keyBytes = Convert.FromBase64String(privateKey
        .Replace("-----BEGIN PRIVATE KEY-----", "")
        .Replace("-----END PRIVATE KEY-----", "")
        .Replace("\n", ""));

    var rsa = RSA.Create();
    rsa.ImportPkcs8PrivateKey(keyBytes, out _);

    var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    var header = new JwtHeader(credentials);
    var token = new JwtSecurityToken(header, new JwtPayload(claims));

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  static async Task<string> GetAccessToken(string jwt)
  {
    using var client = new HttpClient();
    var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
            { "assertion", jwt }
        });

    var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();

    using var jsonDoc = JsonDocument.Parse(jsonResponse);
    return jsonDoc.RootElement.GetProperty("access_token").GetString() ?? string.Empty;
  }

  private static ECDsa LoadECDsaFromPem(string privateKeyPem)
  {
    using (var reader = new StringReader(privateKeyPem))
    {
      var pemReader = new PemReader(reader);
      var ecPrivateKey = pemReader.ReadObject() as ECPrivateKeyParameters;

      if (ecPrivateKey == null)
      {
        throw new Exception("Key is not valid");
      }
      return ConvertToECDsa(ecPrivateKey);
    }
  }
  private static ECDsa ConvertToECDsa(ECPrivateKeyParameters ecPrivateKey)
  {
    var ecParams = new ECParameters
    {
      Curve = ECCurve.NamedCurves.nistP256,
      D = ecPrivateKey.D.ToByteArrayUnsigned()
    };

    var ecDsa = ECDsa.Create();
    ecDsa.ImportParameters(ecParams);
    return ecDsa;
  }
}
