using Microsoft.AspNetCore.Http;
using RackLogic.Core.Interfaces.Models;

namespace RackLogic.Core.Interfaces;
public interface IAzureBlobService
{
  Task<UploadFileAzureBlobResult> UploadFileToBlobAsync(string path, IFormFile file);
  Task<bool> DelteFileToBlobAsync(string path);
  Task<bool> FileExistAsync(string path);
}
