using System.Security.Claims;
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
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();

    if (await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId) is null) return Result.Invalid(new ValidationError("WarehouseId invalid."));

    var inviteIdString = request.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var userCreatedId = Guid.Parse(inviteIdString);
    if (!await CheckPermissionUserRequest(userCreatedId, request.WarehouseId)) return Result.Forbidden();

    var (floorPlans, floorPlanItems, floorPlanComments) = BuildFloorPlanData(request, userCreatedId);

    if (!await SaveFloorPlanData(floorPlans, floorPlanItems, floorPlanComments))
      return Result.Invalid(new ValidationError("Failed to create floor plans."));

    await SaveFloorPlanFiles(floorPlans, request.FloorPlans);

    return Result.Success(true);
  }

  //Funcition
  private (List<FloorPlan>, List<FloorPlanItem>, List<FloorPlanComments>) BuildFloorPlanData(CreateFloorPlanCommand request, Guid userCreatedId)
  {
    var floorPlans = new List<FloorPlan>();
    var floorPlanItems = new List<FloorPlanItem>();
    var floorPlanComments = new List<FloorPlanComments>();

    foreach (var f in request.FloorPlans)
    {
      var floorPlan = new FloorPlan
      {
        Id = f.FloorPlanRequestId ?? Guid.NewGuid(),
        WarehouseId = request.WarehouseId,
        AssetName = f.Title,
        FloorNumber = f.Floor,
        Description = f.Description,
        TotalRack = f.TotalRack,
        TotalBay = f.TotalBay,
        CreatorId = userCreatedId,
        FloorPlanFiles = new List<FloorPlanFiles>(),
        FloorPlanItems = new List<FloorPlanItem>()
      };
      floorPlans.Add(floorPlan);

      foreach (var item in f.Items)
      {
        var floorPlanItem = new FloorPlanItem
        {
          Id = item.Id ?? Guid.NewGuid(),
          FloorPlanId = floorPlan.Id,
          Rack = item.Rack,
          Bay = item.Bay,
          Offset = item.Offset,
          Size = item.Size,
          Width = item.Width,
          Height = item.Height,
          ObjectType = item.ObjectType,
          Rotation = item.Rotation,
          Comments = new List<FloorPlanComments>()
        };
        floorPlanItems.Add(floorPlanItem);

        if (item.Comments != null)
        {
          floorPlanComments.AddRange(item.Comments.Select(comment => new FloorPlanComments
          {
            Id = comment.Id ?? Guid.NewGuid(),
            FloorPlanItemId = floorPlanItem.Id,
            UserName = comment.userName,
            Comment = comment.comment
          }));
        }
      }
    }

    return (floorPlans, floorPlanItems, floorPlanComments);
  }

  private async Task<bool> SaveFloorPlanData(List<FloorPlan> floorPlans, List<FloorPlanItem> floorPlanItems, List<FloorPlanComments> floorPlanComments)
  {
    var savedFloorPlans = await _floorPlanRepository.AddRangeAsync(floorPlans);
    if (savedFloorPlans == null) return false;

    var savedFloorPlanItems = await _floorPlanItemRepository.AddRangeAsync(floorPlanItems);
    if (savedFloorPlanItems == null) return false;

    await _floorPlanCommentsRepository.AddRangeAsync(floorPlanComments);
    return true;
  }

  private async Task SaveFloorPlanFiles(List<FloorPlan> floorPlans, List<FloorPlanCommand> floorPlanRequests)
  {
    foreach (var (floorPlan, f) in floorPlans.Zip(floorPlanRequests))
    {
      if (f.Files?.Any() ?? false)
      {
        var savedFiles = await _fileService.SaveFilesAsync(floorPlan.WarehouseId, floorPlan.Id, f.Files);

        var floorPlanFiles = savedFiles.Select(sf => new FloorPlanFiles
        {
          Id = Guid.NewGuid(),
          FloorPlanId = floorPlan.Id,
          FileName = sf.FileName,
          FileUrl = sf.FilePath,
          ContentType = sf.ContentType
        }).ToList();

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
