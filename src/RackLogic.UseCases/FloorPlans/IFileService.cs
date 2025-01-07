using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.FloorPlans;
public interface IFileService
{
  Task<List<SavedFileInfo>> SaveFilesAsync(Guid warehouseId, Guid floorPlanId, List<IFormFile> files);
  Task<List<string>> DeleteFilesAsync(List<string> fileUrlsToDelete);
  Task<List<SavedFileInfo>> ReplaceFileAsync(Guid warehouseId, Guid floorPlanId, List<IFormFile> fileToUpdate);
  Task<List<FloorPlanDTO>> GetFloorPlansWithFilesByWarehouseIdAsync(Guid warehouseId);
}
