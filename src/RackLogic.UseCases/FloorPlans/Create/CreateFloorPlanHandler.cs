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

    var listFloorPlans = new List<FloorPlan>();
    var listFloorPlanItems = new List<FloorPlanItem>();
    var listFloorPlanComments = new List<FloorPlanComments>();

    foreach (var f in request.FloorPlans)
    {
      // Add FloorPlan
      var floorPlan = new FloorPlan()
      {
        Id = f.FloorPlanRequestId ?? Guid.NewGuid(),
        WarehouseId = request.WarehouseId,
        AssetName = f.Title,
        FloorNumber = f.Floor,
        Description = f.Description,
        totalRack = f.totalRack,
        totalBay = f.totalBay,
        CreatorId = userCreatedId,
        FloorPlanFiles = new List<FloorPlanFiles>(),
        FloorPlanItems = new List<FloorPlanItem>()
      };
      listFloorPlans.Add(floorPlan);

      // Add FloorPlanItem
      foreach (var item in f.Items)
      {
        var floorPlanItem = new FloorPlanItem()
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
        listFloorPlanItems.Add(floorPlanItem);

        // Add FloorPlanComment
        if (item.Comments != null)
        {
          foreach (var comment in item.Comments)
          {
            var floorPlanComment = new FloorPlanComments()
            {
              Id = comment.Id ?? Guid.NewGuid(),
              FloorPlanItemId = floorPlanItem.Id,
              UserName = comment.userName,
              Comment = comment.comment
            };
            listFloorPlanComments.Add(floorPlanComment);
          }
        }
      }
    }

    // Save data to database

    var savedFloorPlans = await _floorPlanRepository.AddRangeAsync(listFloorPlans);
    if (savedFloorPlans == null) return Result.Invalid(new ValidationError("Failed to create floor plans."));

    var savedFloorPlanItems = await _floorPlanItemRepository.AddRangeAsync(listFloorPlanItems);
    if (savedFloorPlanItems == null) return Result.Invalid(new ValidationError("Failed to create floor plan items."));

    var savedFloorPlanComments = await _floorPlanCommentsRepository.AddRangeAsync(listFloorPlanComments);

    //Save files
    foreach (var (floorPlan, f) in savedFloorPlans.Zip(request.FloorPlans))
    {
      // Check if user has uploaded files
      if (f.Files != null && f.Files.Any())
      {
        var savedFiles = await _fileService.SaveFilesAsync(floorPlan.WarehouseId, floorPlan.Id, f.Files);

        // Save file info to database
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

    return Result.Success(true);
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
