using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlans.Update;
public class UpdateFloorPlanHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IUserWarehouseService _userWarehouseService,
  IFloorPlanService _floorPlanService)
  : ICommandHandler<UpdateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(UpdateFloorPlanCommand request, CancellationToken cancellationToken)
  {
    if (!_userWarehouseService.HasPermission(PermissionConstant.FloorPlan.Code, PermissionConstant.FloorPlan.Update))
    {
      return Result.Forbidden(ErrorMessages.FloorPlan.ErrorMessageUpdate);
    }
    // Check if WarehouseId is valid
    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    // Check user role
    if (!await _userWarehouseService.CheckPermissionUserRequestAsync(request.UserCreatedId, request.WarehouseId)) return Result.Forbidden();

    // Validate BayConfigurationId and DefaultSettings
    var validationResult = await _floorPlanService.ValidateBayConfigurationIdAndDefaultSettingsAsync(
        request.FloorAssets.Select(x => x.ToFloorPlanCommand()).ToList(),
        request.WarehouseId,
        cancellationToken
    );
    if (!validationResult.IsSuccess) return validationResult;

    // Use CreateOrUpdateAsync to process both existing and non-existing floor plans
    var createOrUpdateResult = await _floorPlanService.CreateOrUpdateFloorAsssetsAsync(
        request.FloorAssets,
        request.WarehouseId,
        request.UserCreatedId
    );

    if (!createOrUpdateResult.IsSuccess) return createOrUpdateResult;
    return Result.Success(true);
  }
}
