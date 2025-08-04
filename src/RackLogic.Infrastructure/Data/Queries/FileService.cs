using Microsoft.AspNetCore.Http;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.FloorPlans;

namespace RackLogic.Infrastructure.Data.Queries;
public class FileService(IAzureBlobService _azureBlobService) : IFileService
{
  public async Task DeleteFilesAsync(List<string> fileUrlsToDelete)
  {
    var deletTasks = fileUrlsToDelete.Select(_azureBlobService.DelteFileToBlobAsync);
    await Task.WhenAll(deletTasks);
  }

  public async Task<List<SavedFileInfo>> SaveFilesAsync(Guid warehouseId, Guid? floorPlanId, List<IFormFile> files)
  {
    var rootPath = "Warehouse";
    string folderPath = floorPlanId.HasValue ?
                        CombineBlobPath(rootPath, warehouseId.ToString(), "FloorPlan", floorPlanId.Value.ToString()) :
                        CombineBlobPath(rootPath, warehouseId.ToString(), "Logo");

    var tasks = files.Select(s => _azureBlobService.UploadFileToBlobAsync(folderPath, s));
    var taskResults = await Task.WhenAll(tasks);

    return taskResults.Select(s => new SavedFileInfo() 
      { 
        FilePath = s.Url,
        ContentType = s.ContentType,
        FileName = s.FileName
      }).ToList();
  }

  public async Task DeleteFilesByUserIdAsync(Guid userId, string rootPath)
  {
    string folderPath = CombineBlobPath(rootPath, userId.ToString());
    await _azureBlobService.DelteFileToBlobAsync(folderPath);
  }

  public async Task<List<SavedFileInfo>> SaveFilesAsync(Guid id, List<IFormFile> files, string rootPath)
  {
    string folderPath = CombineBlobPath(rootPath, id.ToString());
    var tasks = files.Select(s => _azureBlobService.UploadFileToBlobAsync(folderPath, s));
    var taskResults = await Task.WhenAll(tasks);

    return taskResults.Select(s => new SavedFileInfo()
    {
      FilePath = s.Url,
      ContentType = s.ContentType,
      FileName = s.FileName
    }).ToList();
  }

  private string CombineBlobPath(params string[] parts)
    => string.Join("/", parts.Select(p => p.Trim('/')));
}
