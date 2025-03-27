using Ardalis.Result;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.UseCases.FloorPlans;
public interface IFloorPlanService
{
  Task<List<FloorPlanDTO>> GetFloorPlansWithFilesByWarehouseIdAsync(Guid warehouseId);
  Task<Result<bool>> CreateOrUpdateFloorAsssetsAsync(
    List<UpdateFloorPlanInformationCommand> floorAssets,
    Guid warehouseId,
    Guid userCreatedId);
  Task<Result<bool>> ValidateBayConfigurationIdAndDefaultSettingsAsync(
    List<FloorPlanCommand> floorPlans,
    Guid warehouseId,
    CancellationToken cancellationToken);
  Task<List<FloorPlanFiles>> SaveFloorPlanFilesAsync(List<FloorPlanCommand> floorPlanRequests, Guid warehouseId);
}
