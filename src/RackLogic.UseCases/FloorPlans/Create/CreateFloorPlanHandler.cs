using System.Text.RegularExpressions;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlans.Create;
public class CreateFloorPlanHandler(
  IRepositoryWithExpression<FloorPlan> _floorPlanRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFloorPlanService _floorPlanService,
  IUserWarehouseService _userWarehouseService)
  : ICommandHandler<CreateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateFloorPlanCommand request, CancellationToken cancellationToken)
  {
    // Check validity of WarehouseId
    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    // Check if the warehouse has a floor plan
    if (await _floorPlanRepository.FirstOrDefaultAsync(x => x.WarehouseId == request.WarehouseId) != null) return Result.Invalid(new ValidationError("This Warehouse already has a Floor Plan!"));

    // Check user permissions
    if (!await _userWarehouseService.CheckPermissionUserRequestAsync(request.UserCreatedId, request.WarehouseId)) return Result.Forbidden();

    // Validate BayConfigurationId and DefaultSettings
    var validationResult = await _floorPlanService.ValidateBayConfigurationIdAndDefaultSettingsAsync(
      request.FloorPlans, 
      request.WarehouseId, 
      cancellationToken);

    if (!validationResult.IsSuccess) return validationResult;

    return await AddFloorPlanAsync(request, request.WarehouseId, cancellationToken);
  }

  private async Task<Result<bool>> AddFloorPlanAsync(CreateFloorPlanCommand request, Guid warehouseId, CancellationToken cancellationToken)
  {

    try
    {
      //Mapping FloorPlan and FloorPlanFiles
      var floorPlans = request.FloorPlans.Select(x => x.ToFloorPlan(warehouseId));
      //Save Files
      var floorPlanFiles = await _floorPlanService.SaveFloorPlanFilesAsync(request.FloorPlans, warehouseId);
      floorPlans = floorPlans.ToList().MapFloorPlanFilesToFloorPlan(floorPlanFiles);
      var results = await _floorPlanRepository.AddRangeAsync(floorPlans, cancellationToken);
      return results.Any();
    }
    catch (DbUpdateException ex)
    {
      var errorMessage = ex?.InnerException?.Message;

      //Use regex to find GUID in error message
      var match = Regex.Match(errorMessage ?? "", @"\(([^)]+)\)");
      if (match.Success && Guid.TryParse(match.Groups[1].Value, out Guid duplicateId))
      {
        return Result.Invalid(new ValidationError($"Duplicate Id: {duplicateId}"));
      }

      return Result.Invalid(new ValidationError(errorMessage));
    }
  }
}
