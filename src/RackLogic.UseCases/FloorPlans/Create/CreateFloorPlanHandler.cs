using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Create;
public class CreateFloorPlanHandler(
  IRepositoryWithExpression<FloorPlan> _floorPlanRepository,
  IRepositoryWithExpression<FloorPlanFiles> _floorPlanFilesRepository,
  IRepositoryWithExpression<FloorPlanItem> _floorPlanItemRepository,
  IRepositoryWithExpression<FloorPlanComments> _floorPlanCommentsRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFileService _fileService,
  RoleManager<IdentityRole<Guid>> _roleManager)
  : ICommandHandler<CreateFloorPlanCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateFloorPlanCommand request, CancellationToken cancellationToken)
  {

    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    var userCreatedId = request.UserCreatedId;
    if (!await CheckPermissionUserRequest(userCreatedId, request.WarehouseId)) return Result.Forbidden();

    var (floorPlans, floorPlanItems, floorPlanComments) = GetFloorPlanData(request, userCreatedId);

    var saveResult = await SaveFloorPlanData(floorPlans, floorPlanItems, floorPlanComments, request.FloorPlans);
    if (!saveResult.IsSuccess) return saveResult;

    return Result.Success(true);
  }

  //Funcition
  private (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) GetFloorPlanData(CreateFloorPlanCommand request, Guid userCreatedId)
  {
    var floorPlans = request.FloorPlans
        .Select(f => f.ToFloorPlan(request.WarehouseId, userCreatedId))
        .ToList();

    var floorPlanItems = request.FloorPlans
        .SelectMany(f => f.Items, (f, item) => item.ToFloorPlanItem(f.FloorPlanRequestId))
        .ToList();

    var floorPlanComments = request.FloorPlans
        .SelectMany(f => f.Items)
        .Where(item => item.Comments != null)
        .SelectMany(item => item.Comments ?? new List<FloorPlanCommentCommand>(), (item, comment) => comment.ToFloorPlanComment(item.Id))
        .ToList();

    return (floorPlans, floorPlanItems, floorPlanComments);
  }
  private async Task<Result<bool>> SaveFloorPlanData(List<FloorPlan> floorPlans, List<FloorPlanItem> floorPlanItems, List<FloorPlanComments> floorPlanComments, List<FloorPlanCommand> floorPlanRequests)
  {
    // // Check errors for each object
    var errorFloorPlanIds = await CheckAndGetErrorsForFloorPlans(floorPlans);
    var errorFloorPlanItemIds = await CheckAndGetErrorsForFloorPlanItems(floorPlanItems, errorFloorPlanIds);
    var errorFloorPlanCommentIds = await CheckAndGetErrorsForFloorPlanComments(floorPlanComments, errorFloorPlanItemIds);

    // Save valid floorPlans to DB first
    var validFloorPlans = floorPlans.Where(f => !errorFloorPlanIds.Contains(f.Id)).ToList();
    await _floorPlanRepository.AddRangeAsync(validFloorPlans);

    // Save valid floorPlanItems to DB first
    var validfloorPlanItems = floorPlanItems.Where(i => !errorFloorPlanItemIds.Contains(i.Id)).ToList();
    await _floorPlanItemRepository.AddRangeAsync(validfloorPlanItems);

    // Save valid floorPlanComments to DB first
    var validfloorPlanComments = floorPlanComments.Where(c => !errorFloorPlanCommentIds.Contains(c.Id)).ToList();
    await _floorPlanCommentsRepository.AddRangeAsync(validfloorPlanComments);

    // Check and return errors if any
    var validationErrors = new List<ValidationError>();

    if (errorFloorPlanIds.Any())
    {
      validationErrors.Add(new ValidationError($"Failed to create floor plans with IDs: {string.Join(", ", errorFloorPlanIds)}"));
    }
    if (errorFloorPlanItemIds.Any())
    {
      validationErrors.Add(new ValidationError($"Failed to create items with IDs: {string.Join(", ", errorFloorPlanItemIds)}"));
    }
    if (errorFloorPlanCommentIds.Any())
    {
      validationErrors.Add(new ValidationError($"Failed to create comments with IDs: {string.Join(", ", errorFloorPlanCommentIds)}"));
    }

    await SaveFloorPlanFiles(floorPlans, floorPlanRequests);

    if (validationErrors.Any()) return Result<bool>.Invalid(validationErrors);

    return Result.Success(true);
  }

  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlans(List<FloorPlan> floorPlans)
  {
    var floorPlanIds = floorPlans.Select(f => f.Id).ToList();

    var existingFloorPlans = await _floorPlanRepository.ListAsync(f => floorPlanIds.Contains(f.Id));

    return existingFloorPlans.Select(f => f.Id).ToList();
  }


  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlanItems(List<FloorPlanItem> floorPlanItems, List<Guid> errorFloorPlanIds)
  {
    var floorPlanItemIds = new HashSet<Guid>(floorPlanItems.Select(f => f.Id));
    var existingFloorPlanItems = await _floorPlanItemRepository.ListAsync(f => floorPlanItemIds.Contains(f.Id));
    var existingFloorPlanItemIds = new HashSet<Guid>(existingFloorPlanItems.Select(e => e.Id));

    var errorFloorPlanIdsSet = new HashSet<Guid>(errorFloorPlanIds);

    var errorFloorPlanItemIds = floorPlanItems
        .Where(floorPlanItem => 
          errorFloorPlanIdsSet.Contains(floorPlanItem.FloorPlanId) || 
          existingFloorPlanItemIds.Contains(floorPlanItem.Id))
        .Select(floorPlanItem => floorPlanItem.Id)
        .ToList();

    return errorFloorPlanItemIds;
  }


  private async Task<List<Guid>> CheckAndGetErrorsForFloorPlanComments(List<FloorPlanComments> floorPlanComments, List<Guid> errorFloorPlanItemIds)
  {
    var floorPlanCommentIds = new HashSet<Guid>(floorPlanComments.Select(f => f.Id));
    var existingFloorPlanComments = await _floorPlanCommentsRepository.ListAsync(f => floorPlanCommentIds.Contains(f.Id));
    var existingFloorPlanCommentIds = new HashSet<Guid>(existingFloorPlanComments.Select(e => e.Id));

    var errorFloorPlanItemIdsSet = new HashSet<Guid>(errorFloorPlanItemIds);

    var errorFloorPlanCommentIds = floorPlanComments
        .Where(floorPlanComment =>
            errorFloorPlanItemIdsSet.Contains(floorPlanComment.FloorPlanItemId) ||
            existingFloorPlanCommentIds.Contains(floorPlanComment.Id))
        .Select(floorPlanComment => floorPlanComment.Id)
        .ToList();

    return errorFloorPlanCommentIds;
  }

  private async Task SaveFloorPlanFiles(List<FloorPlan> floorPlans, List<FloorPlanCommand> floorPlanRequests)
  {
    foreach (var (floorPlan, f) in floorPlans.Zip(floorPlanRequests))
    {
      if (f.Files?.Any() ?? false)
      {
        var savedFiles = await _fileService.SaveFilesAsync(floorPlan.WarehouseId, floorPlan.Id, f.Files);
        var floorPlanFiles = savedFiles
            .Select(sf => sf.ToFloorPlanFile(floorPlan.Id))
            .ToList();

        await _floorPlanFilesRepository.AddRangeAsync(floorPlanFiles);
      }
    }
  }

  private async Task<bool> CheckPermissionUserRequest(Guid userId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises &&
      role.Name != RoleConstant.ClientManager &&
      role.Name != RoleConstant.Inspector) return false;

    return true;
  }
}
