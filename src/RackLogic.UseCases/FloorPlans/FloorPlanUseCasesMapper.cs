using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.UseCases.FloorPlans;
public static partial class FloorPlanUseCasesMapper
{
  public static FloorPlan ToFloorPlan(this FloorPlanCommand request, Guid warehouseId, Guid userCreatedId)
  {
    return new FloorPlan
    {
      Id = request.FloorPlanRequestId,
      WarehouseId = warehouseId,
      AssetName = request.Title,
      FloorNumber = request.Floor,
      Description = request.Description,
      TotalRack = request.TotalRack,
      TotalBay = request.TotalBay,
      CreatorId = userCreatedId,
      FloorPlanFiles = new List<FloorPlanFiles>(),
      FloorPlanItems = new List<FloorPlanItem>()
    };
  }

  public static FloorPlanItem ToFloorPlanItem(this FloorPlanItemCommand item, Guid floorPlanId)
  {
    return new FloorPlanItem
    {
      Id = item.Id ,
      FloorPlanId = floorPlanId,
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
  }

  public static FloorPlanComments ToFloorPlanComment(this FloorPlanCommentCommand comment, Guid floorPlanItemId)
  {
    return new FloorPlanComments
    {
      Id = comment.Id,
      FloorPlanItemId = floorPlanItemId,
      UserName = comment.userName,
      Comment = comment.comment
    };
  }

  public static FloorPlanFiles ToFloorPlanFile(this SavedFileInfo savedFile, Guid floorPlanId)
  {
    return new FloorPlanFiles
    {
      Id = Guid.NewGuid(),
      FloorPlanId = floorPlanId,
      FileName = savedFile.FileName,
      FileUrl = savedFile.FilePath,
      ContentType = savedFile.ContentType
    };
  }

  public static void UpdateFloorPlan(this UpdateFloorPlanInformationCommand request, FloorPlan floorPlan)
  {
    floorPlan.AssetName = request.Title;
    floorPlan.FloorNumber = request.Floor;
    floorPlan.Description = request.Description;
    floorPlan.TotalRack = request.TotalRack;
    floorPlan.TotalBay = request.TotalBay;
  }

  public static void UpdateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, FloorPlanItem floorPlanItem)
  {
    floorPlanItem.Rack = item.Rack;
    floorPlanItem.Bay = item.Bay;
    floorPlanItem.Offset = item.Offset;
    floorPlanItem.Size = item.Size;
    floorPlanItem.Width = item.Width;
    floorPlanItem.Height = item.Height;
    floorPlanItem.ObjectType = item.ObjectType;
    floorPlanItem.Rotation = item.Rotation;
  }

  public static void UpdateFloorPlanComment(this UpdateFloorPlanCommentCommand comment, FloorPlanComments floorPlanComment)
  {
    floorPlanComment.UserName = comment.UserName;
    floorPlanComment.Comment = comment.Comment;
    floorPlanComment.CreationTime = DateTime.Parse(comment.Date);
  }

}
