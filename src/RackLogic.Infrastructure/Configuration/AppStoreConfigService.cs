namespace RackLogic.Infrastructure.Configuration;
public class AppStoreConfigService
{
  public AppStoreConfig GetConfig()
  {
    return new AppStoreConfig
    {
      Audience = Environment.GetEnvironmentVariable("AUDIENCE", EnvironmentVariableTarget.User) ?? string.Empty,
      BundleId = Environment.GetEnvironmentVariable("BUNDLE_ID", EnvironmentVariableTarget.User) ?? string.Empty,
      IssuerId = Environment.GetEnvironmentVariable("ISSUER_ID", EnvironmentVariableTarget.User) ?? string.Empty,
      KeyId = Environment.GetEnvironmentVariable("KEY_ID", EnvironmentVariableTarget.User) ?? string.Empty,
      PrivateKey = Environment.GetEnvironmentVariable("PRIVATE_KEY_APPSTORE", EnvironmentVariableTarget.User) ?? string.Empty
    };
  }
}
