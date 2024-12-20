using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace RackLogic.Web.Providers;

public class RefreshTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
{
  public RefreshTokenProvider(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<RefreshTokenDataProtectionTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger)
    : base(dataProtectionProvider, options, logger)
  {
  }
}

public class RefreshTokenDataProtectionTokenProviderOptions : DataProtectionTokenProviderOptions { }
