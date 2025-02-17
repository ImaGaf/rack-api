using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

  public async Task<List<FloorPlanDTO>> GetFloorPlansWithFilesByWarehouseIdAsync(Guid warehouseId)
  {

    //Get list FloorPlans by WarehouseId
    var floorPlans = await _context.FloorPlans
        .Include(f => f.FloorPlanFiles)
        .Include(f => f.FloorPlanItems)
            .ThenInclude(i => i.Comments)
        .Where(f => f.WarehouseId == warehouseId)
        .ToListAsync();

    return floorPlans.Select(f => new FloorPlanDTO
    {
      FloorPlanRequestId = f.Id,
      Title = f.AssetName,
      Floor = f.FloorNumber,
      Description = f.Description,
      totalRack = f.totalRack ,
      totalBay = f.totalBay ,
      FileUrls = f.FloorPlanFiles.Select(file => file.FileUrl).ToList(),
      Items = f.FloorPlanItems.Select(i => new FloorPlanItemResponse
      {
        Id = i.Id,
        Rack = i.Rack,
        Bay = i.Bay,
        Offset = i.Offset,
        Size = i.Size,
        Width = i.Width,
        Height = i.Height,
        ObjectType = i.ObjectType,
        Rotation = i.Rotation,
        Comments = i.Comments?.Select(c => new FloorPlanCommentResponse
        {
          Id = c.Id,
          UserName = c.UserName,
          Comment = c.Comment,
          Date = c.CreationTime.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList() ?? new List<FloorPlanCommentResponse>()
      }).ToList()
    }).ToList();
  }

  public async Task<List<SavedFileInfo>> ReplaceFileAsync(Guid warehouseId, Guid floorPlanId, List<IFormFile> filesToUpdate)
  {
    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Warehouse");
    var folderPath = Path.Combine(rootPath, warehouseId.ToString(), "FloorPlan", floorPlanId.ToString());

    var updatedFilesInfo = new List<SavedFileInfo>();

    foreach (var file in filesToUpdate)
    {
      var fileName = Path.GetFileName(file.FileName);
      var filePath = Path.Combine(folderPath, fileName);

      if (!File.Exists(filePath))
      {
        continue;
      }

      // Delete exist file in folder
      File.Delete(filePath);

      // Add new file to folder
      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      updatedFilesInfo.Add(new SavedFileInfo
      {
        FileName = fileName,
        FilePath = $"Warehouse/{warehouseId}/FloorPlan/{floorPlanId}/{fileName}",
        ContentType = file.ContentType
      });
    }

    return updatedFilesInfo;
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
  }
