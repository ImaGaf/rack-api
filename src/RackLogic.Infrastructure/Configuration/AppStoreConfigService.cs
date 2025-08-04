using RackLogic.Core.Common.Constants;

namespace RackLogic.Infrastructure.Configuration;
public class AppStoreConfigService
{
  public AppStoreConfig GetConfig()
  {
    return new AppStoreConfig
    {
      Audience = Environment.GetEnvironmentVariable(EnvironmentConstant.Audience)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.Audience, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.Audience, EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      BundleId = Environment.GetEnvironmentVariable(EnvironmentConstant.BundleId) 
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.BundleId, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.BundleId, EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      IssuerId = Environment.GetEnvironmentVariable(EnvironmentConstant.IssuerId)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.IssuerId, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.IssuerId, EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      KeyId = Environment.GetEnvironmentVariable(EnvironmentConstant.KeyId)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.KeyId, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.KeyId, EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      PrivateKey = Environment.GetEnvironmentVariable(EnvironmentConstant.PrivateKey)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.PrivateKey, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.PrivateKey, EnvironmentVariableTarget.Machine)
        ?? string.Empty
    };
  }
}
