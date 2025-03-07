using Ardalis.Result;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.UseCases.FloorPlans;
public interface IFloorPlanService
{
  Task<List<FloorPlanDTO>> GetFloorPlansWithFilesByWarehouseIdAsync(Guid warehouseId);
  (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanData(CreateFloorPlanCommand request, Guid userCreatedId);
  Task<Result<bool>> SaveFloorPlanDataAsync(List<FloorPlan> floorPlans, List<FloorPlanItem> floorPlanItems, List<FloorPlanComments> floorPlanComments, List<FloorPlanCommand> floorPlanRequests);
  Task SaveFloorPlanFilesAsync(List<FloorPlan> floorPlans, List<FloorPlanCommand> floorPlanRequests);
  (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanDataForUpdate(List<UpdateFloorPlanInformationCommand> nonMatchingPlans, Guid warehouseId, Guid userCreatedId);
  Task<(List<UpdateFloorPlanInformationCommand> existFloorAssetsDb, List<UpdateFloorPlanInformationCommand> notExistFloorAssetsDb)> GetExistingAndNewFloorPlansAsync(UpdateFloorPlanCommand request);
  Task<Result<bool>> ProcessExistingFloorPlansAsync(List<UpdateFloorPlanInformationCommand> existFloorAssetsDb, Guid warehouseId);
  Task<Result<bool>> CreateNewFloorPlansAsync(List<UpdateFloorPlanInformationCommand> notExistFloorAssetsDb, Guid warehouseId, Guid userCreatedId);
}
