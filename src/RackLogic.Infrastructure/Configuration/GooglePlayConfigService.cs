
using Newtonsoft.Json;
using RackLogic.Core.Common.Constants;

namespace RackLogic.Infrastructure.Configuration;
public class GooglePlayConfigService
{
  public GooglePlayConfig GetConfig()
  {
    var path = Environment.GetEnvironmentVariable(EnvironmentConstant.GGKeyPath)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.GGKeyPath, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.GGKeyPath, EnvironmentVariableTarget.Machine);

    if (string.IsNullOrEmpty(path)) return new GooglePlayConfig();

    string jsonContent = string.Empty;

    try
    {
      if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
      {
        using (var httpClient = new HttpClient())
        {
          jsonContent = httpClient.GetStringAsync(path).Result;
        }
      }
      else
      {
        jsonContent = File.ReadAllText(path);
      }

      var credentials = JsonConvert.DeserializeObject<GooglePlayConfig>(jsonContent);
      return credentials ?? new GooglePlayConfig();
    }
    catch
    {
      return new GooglePlayConfig();
    }
  }
}
