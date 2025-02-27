namespace RackLogic.Infrastructure.Configuration;
public class AppStoreConfigService
{
  public AppStoreConfig GetConfig()
  {
    return new AppStoreConfig
    {
      Audience = Environment.GetEnvironmentVariable("AUDIENCE", EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable("AUDIENCE", EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      BundleId = Environment.GetEnvironmentVariable("BUNDLE_ID", EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable("BUNDLE_ID", EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      IssuerId = Environment.GetEnvironmentVariable("ISSUER_ID", EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable("ISSUER_ID", EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      KeyId = Environment.GetEnvironmentVariable("KEY_ID", EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable("KEY_ID", EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      PrivateKey = Environment.GetEnvironmentVariable("PRIVATE_KEY_APPSTORE", EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable("PRIVATE_KEY_APPSTORE", EnvironmentVariableTarget.Machine)
        ?? string.Empty
    };
  }
}
