using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using RackLogic.Core.Interfaces;
using RackLogic.Core.Interfaces.Models;
using RackLogic.Infrastructure.Configuration;

namespace RackLogic.Infrastructure.AzureBlobFile;
public class AzureBlobService(AppConfigService _appConfigService) : IAzureBlobService
{
  public async Task<bool> DelteFileToBlobAsync(string path)
  {
    var configs = _appConfigService.GetConfig();
    var blobServiceClient = new BlobServiceClient(configs.AzureBlobConnectionString);
    var blobContainer = blobServiceClient.GetBlobContainerClient(configs.BlobContainerName);
    var blobClient = blobContainer.GetBlobClient(path);

    var response = await blobClient.DeleteIfExistsAsync();

    return response.Value;
  }

  public async Task<bool> FileExistAsync(string path)
  {
    var configs = _appConfigService.GetConfig();
    var blobServiceClient = new BlobServiceClient(configs.AzureBlobConnectionString);
    var blobContainer = blobServiceClient.GetBlobContainerClient(configs.BlobContainerName);
    var blobClient = blobContainer.GetBlobClient(path);

    var response = await blobClient.ExistsAsync();
    return response.Value;
  }

  public async Task<UploadFileAzureBlobResult> UploadFileToBlobAsync(string path, IFormFile file)
  {
    var configs = _appConfigService.GetConfig();
    var blobServiceClient = new BlobServiceClient(configs.AzureBlobConnectionString);
    var blobContainer = blobServiceClient.GetBlobContainerClient(configs.BlobContainerName);

    await blobContainer.CreateIfNotExistsAsync();

    var blobName = Path.Combine(path,file.Name);
    var blobClient = blobContainer.GetBlobClient(blobName);

    using (var stream = file.OpenReadStream())
    {
      await blobClient.UploadAsync(stream, overwrite: true);
    }

    return new UploadFileAzureBlobResult() {
      Url = blobClient.Uri.ToString(),
      FileName = file.Name,
      ContentType = file.ContentType
    };
  }
}
