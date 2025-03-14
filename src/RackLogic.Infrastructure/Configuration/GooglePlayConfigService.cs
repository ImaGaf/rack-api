
using Newtonsoft.Json;

namespace RackLogic.Infrastructure.Configuration;
public class GooglePlayConfigService
{
  public GooglePlayConfig GetConfig()
  {
    var path = Environment.GetEnvironmentVariable("PLAY_KEY_PATH", EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable("PLAY_KEY_PATH", EnvironmentVariableTarget.Machine);

    if (string.IsNullOrEmpty(path)) return new GooglePlayConfig();

    var credentials = JsonConvert.DeserializeObject<GooglePlayConfig>(File.ReadAllText(path) ?? string.Empty);

    if (credentials == null) return new GooglePlayConfig();

    return credentials;
  }
}
