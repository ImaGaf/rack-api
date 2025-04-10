using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.FloorPlans;
public interface IFileService
{
  Task<List<SavedFileInfo>> SaveFilesAsync(Guid warehouseId, Guid? floorPlanId, List<IFormFile> files);
  Task<List<string>> DeleteFilesAsync(List<string> fileUrlsToDelete);
  Task<List<string>> DeleteFilesByUserIdAsync(Guid userId, string rootPath);
  Task<List<SavedFileInfo>> SaveFilesAsync(Guid id, List<IFormFile> files, string rootPath);
}
