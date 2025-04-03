using Microsoft.AspNetCore.Http;
using RackLogic.UseCases.FloorPlans;

namespace RackLogic.Infrastructure.Data.Queries;
public class FileService : IFileService
{
  private readonly AppDbContext _context;

  public FileService(AppDbContext context)
  {
    _context = context;
  }
  public Task<List<string>> DeleteFilesAsync(List<string> fileUrlsToDelete)
  {
    string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    var nonExistentFiles = new List<string>();

    foreach (var fileUrl in fileUrlsToDelete)
    {
      string fullFilePath = Path.Combine(rootPath, fileUrl);

      if (File.Exists(fullFilePath))
      {
        File.Delete(fullFilePath);
      }

      else
      {
        nonExistentFiles.Add(fullFilePath);
      }
    }
    return Task.FromResult(nonExistentFiles);
  }

  public async Task<List<SavedFileInfo>> SaveFilesAsync(Guid warehouseId, Guid? floorPlanId, List<IFormFile> files)
  {
    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Warehouse");
    string folderPath;
    if (floorPlanId.HasValue)
    {
      // Case when floorPlanId is not null
      folderPath = Path.Combine(rootPath, warehouseId.ToString(), "FloorPlan", floorPlanId.Value.ToString());
    }
    else
    {
      // Case when floorPlanId is null
      folderPath = Path.Combine(rootPath, warehouseId.ToString(), "Logo");
    }

    if (!Directory.Exists(folderPath))
    {
      Directory.CreateDirectory(folderPath);
    }

    var fileInfoList = new List<SavedFileInfo>();

    foreach (var file in files)
    {
      var fileName = Path.GetFileName(file.FileName);
      var filePath = Path.Combine(folderPath, fileName);

      if (File.Exists(filePath))
      {
        continue;
      }
      // Save file to folder
      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      // Save info file to list
      fileInfoList.Add(new SavedFileInfo
      {
        FilePath = floorPlanId.HasValue
              ? $"Warehouse/{warehouseId}/FloorPlan/{floorPlanId}/{fileName}"
              : $"Warehouse/{warehouseId}/Logo/{fileName}",
        FileName = fileName,
        ContentType = file.ContentType
      });
    }

    return fileInfoList;
  }

  public Task<List<string>> DeleteFilesByUserIdAsync(Guid userId, string rootPath)
  {
    string folderPath = Path.Combine(rootPath, userId.ToString());
    var nonExistentFiles = new List<string>();

    if (Directory.Exists(folderPath))
    {
      var files = Directory.GetFiles(folderPath);
      foreach (var file in files)
      {
        File.Delete(file);
      }
    }
    else
    {
      nonExistentFiles.Add(folderPath);
    }
    return Task.FromResult(nonExistentFiles);
  }

  public async Task<List<SavedFileInfo>> SaveFilesByUserIdAsync(Guid userId, List<IFormFile> files, string rootPath)
  {
    string folderName = Path.GetFileName(rootPath);
    string folderPath = Path.Combine(rootPath, userId.ToString());

    if (!Directory.Exists(folderPath))
    {
      Directory.CreateDirectory(folderPath);
    }

    var fileInfoList = new List<SavedFileInfo>();

    foreach (var file in files)
    {
      var fileName = Path.GetFileName(file.FileName);
      var filePath = Path.Combine(folderPath, fileName);

      if (File.Exists(filePath))
      {
        continue;
      }

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      fileInfoList.Add(new SavedFileInfo
      {
        FilePath = $"{folderName}/{userId}/{fileName}",
        FileName = fileName,
        ContentType = file.ContentType
      });
    }

    return fileInfoList;
  }
}
