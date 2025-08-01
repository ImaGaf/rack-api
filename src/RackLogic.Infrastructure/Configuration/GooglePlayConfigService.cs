
using Newtonsoft.Json;
using RackLogic.Core.Common.Constants;

namespace RackLogic.Infrastructure.Configuration;
public class GooglePlayConfigService
{
  public GooglePlayConfig GetConfig()
  {
    var path = Environment.GetEnvironmentVariable(EnvironmentConstant.GGKeyPath, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.GGKeyPath, EnvironmentVariableTarget.Machine);

    if (string.IsNullOrEmpty(path)) return new GooglePlayConfig();

    var credentials = JsonConvert.DeserializeObject<GooglePlayConfig>(File.ReadAllText(path) ?? string.Empty);

    if (credentials == null) return new GooglePlayConfig();

    return credentials;
  }
}
