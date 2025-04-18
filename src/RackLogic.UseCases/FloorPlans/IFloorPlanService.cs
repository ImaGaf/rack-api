using Ardalis.Result;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.FloorPlanItems.GetDetail;
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
  Task<FloorPlanDTO?> GetDetailAsync(Guid wareHouseId, Guid floorPlanId, CancellationToken cancellationToken = default);
  Task<IEnumerable<FloorPlanItemDto>?> GetDetailDefectLogAsync(Guid floorPlanId, CancellationToken cancellationToken);
}
