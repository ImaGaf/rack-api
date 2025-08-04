namespace RackLogic.Infrastructure.Configuration;
public class AppConfig
{
  public required string ConnectionString { get; set; }
  public required string AzureBlobConnectionString { get; set; }
  public required string BlobContainerName { get; set; }
}
