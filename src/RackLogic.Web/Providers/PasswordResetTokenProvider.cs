using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace RackLogic.Web.Providers;
public class PasswordResetTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
{
  public PasswordResetTokenProvider(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<PasswordResetTokenDataProtectionTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger)
    : base(dataProtectionProvider, options, logger)
  {
  }
}

public class PasswordResetTokenDataProtectionTokenProviderOptions : DataProtectionTokenProviderOptions { }
