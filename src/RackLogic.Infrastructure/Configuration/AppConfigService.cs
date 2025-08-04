using RackLogic.Core.Common.Constants;

namespace RackLogic.Infrastructure.Configuration;
public class AppConfigService
{
  public AppConfig GetConfig()
  {
    return new AppConfig()
    {
      ConnectionString = Environment.GetEnvironmentVariable(EnvironmentConstant.ConnectionString)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.ConnectionString, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.ConnectionString, EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      AzureBlobConnectionString = Environment.GetEnvironmentVariable(EnvironmentConstant.AZBlobConnectionString)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.AZBlobConnectionString, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.AZBlobConnectionString, EnvironmentVariableTarget.Machine)
        ?? string.Empty,
      BlobContainerName = Environment.GetEnvironmentVariable(EnvironmentConstant.AZBlobContainerName)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.AZBlobContainerName, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(EnvironmentConstant.AZBlobContainerName, EnvironmentVariableTarget.Machine)
        ?? string.Empty,
    };
  }
}
