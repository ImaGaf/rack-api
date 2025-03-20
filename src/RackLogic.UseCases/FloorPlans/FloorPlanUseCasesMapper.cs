using Microsoft.AspNetCore.Http;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.UseCases.FloorPlans;
public static partial class FloorPlanUseCasesMapper
{
  public static FloorPlan ToFloorPlan(this FloorPlanCommand request, Guid warehouseId)
  {
    return new FloorPlan
    {
      Id = request.FloorPlanId,
      WarehouseId = warehouseId,
      AssetName = request.Title,
      FloorNumber = request.Floor,
      Description = request.Description,
      TotalRack = request.TotalRack,
      TotalBay = request.TotalBay,
      FloorPlanFiles = new List<FloorPlanFiles>(),
      FloorPlanItems = new List<FloorPlanItem>(),
      DefaultSettings = new List<DefaultSettings>()
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
      BayConfigurationId = item.BayConfigurationId,
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
  // Mapper FloorPlanAsssets to UpdateFloorPlanInformationCommand
  public static FloorPlan ToFloorPlan(this UpdateFloorPlanInformationCommand request, Guid warehouseId, Guid userCreatedId)
  {
    return new FloorPlan
    {
      Id = request.FloorPlanId,
      WarehouseId = warehouseId,
      AssetName = request.Title,
      FloorNumber = request.Floor,
      Description = request.Description,
      TotalRack = request.TotalRack,
      TotalBay = request.TotalBay,
      CreatorId = userCreatedId,
      FloorPlanFiles = new List<FloorPlanFiles>(),
      FloorPlanItems = new List<FloorPlanItem>(),
      DefaultSettings = new List<DefaultSettings>() 
    };
  }
  public static FloorPlanItem ToFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, Guid floorPlanId)
  {
    return new FloorPlanItem
    {
      Id = item.Id,
      FloorPlanId = floorPlanId,
      Rack = item.Rack,
      Bay = item.Bay,
      Offset = item.Offset,
      Size = item.Size,
      Width = item.Width,
      Height = item.Height,
      ObjectType = item.ObjectType,
      Rotation = item.Rotation,
      BayConfigurationId = item.BayConfigurationId,
      Comments = new List<FloorPlanComments>()
    };
  }
  //Create new items for update FloorPlanItems in FloorPlanAsset
  public static FloorPlanItem ToCreateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, Guid floorPlanId)
  {
     var comments = item.Comments?.Select(c => c.ToFloorPlanComment(item.Id)).ToList() ?? new List<FloorPlanComments>();
    return new FloorPlanItem
    {
      Id = item.Id,
      FloorPlanId = floorPlanId,
      Rack = item.Rack,
      Bay = item.Bay,
      Offset = item.Offset,
      Size = item.Size,
      Width = item.Width,
      Height = item.Height,
      ObjectType = item.ObjectType,
      Rotation = item.Rotation,
      BayConfigurationId = item.BayConfigurationId,
      Comments = comments
    };
  }


  public static FloorPlanComments ToFloorPlanComment(this UpdateFloorPlanCommentCommand comment, Guid floorPlanItemId)
  {
    return new FloorPlanComments
    {
      Id = comment.Id,
      FloorPlanItemId = floorPlanItemId,
      UserName = comment.UserName,
      Comment = comment.Comment,
      CreationTime = DateTime.Parse(comment.Date)
    };
  }

  //Mapper UpdateFloorPlanInformationCommand to FloorPlanCommand
  public static FloorPlanCommand ToFloorPlanCommand(this UpdateFloorPlanInformationCommand request)
  {
    return new FloorPlanCommand(
        FloorPlanId: request.FloorPlanId,
        Title: request.Title,
        Floor: request.Floor,
        Description: request.Description,
        TotalRack: request.TotalRack,
        TotalBay: request.TotalBay,
        Items: request.Items?.Select(i => i.ToFloorPlanItemCommand()).ToList() ?? new List<FloorPlanItemCommand>(),
        Files: request.Files ?? new List<IFormFile>(),
        DefaultSettings: new DefaultSettingsCommand(
            SingleBin: request.UpdateDefaultSettings.SingleBin,
            PairSingleBin: request.UpdateDefaultSettings.PairSingleBin,
            DoubleBin: request.UpdateDefaultSettings.DoubleBin,
            PairDoubleBin: request.UpdateDefaultSettings.PairDoubleBin,
            TripleBin: request.UpdateDefaultSettings.TripleBin,   
            PairTripleBin: request.UpdateDefaultSettings.PairTripleBin,
            QuadBin: request.UpdateDefaultSettings.QuadBin,
            PairQuadBin: request.UpdateDefaultSettings.PairQuadBin  
        )
    );
  }

  public static FloorPlanItemCommand ToFloorPlanItemCommand(this UpdateFloorPlanItemInformationCommand item)
  {
    return new FloorPlanItemCommand(
        Id: item.Id,
        Rack: item.Rack,
        Bay: item.Bay,
        Offset: item.Offset,
        Size: item.Size,
        Width: item.Width,
        Height: item.Height,
        ObjectType: item.ObjectType,
        Rotation: item.Rotation,
        BayConfigurationId: item.BayConfigurationId, 
        Comments: item.Comments?.Select(c => c.ToFloorPlanCommentCommand()).ToList() ?? new List<FloorPlanCommentCommand>()
    );
  }

  public static FloorPlanCommentCommand ToFloorPlanCommentCommand(this UpdateFloorPlanCommentCommand comment)
  {
    return new FloorPlanCommentCommand(
        Id: comment.Id,
        userName: comment.UserName,
        comment: comment.Comment
    );
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

  public static FloorPlanDTO ToFloorPlanDTO(this FloorPlan floorPlan)
  {
    return new FloorPlanDTO
    {
      FloorPlanId = floorPlan.Id,
      Title = floorPlan.AssetName,
      Floor = floorPlan.FloorNumber,
      Description = floorPlan.Description,
      TotalRack = floorPlan.TotalRack,
      TotalBay = floorPlan.TotalBay,
      FileUrls = floorPlan.FloorPlanFiles.Select(file => file.FileUrl).ToList(),
      Items = floorPlan.FloorPlanItems.Select(i => i.ToFloorPlanItemResponse()).ToList()
    };
  }

  // Map from FloorPlanItem to FloorPlanItemResponse
  public static FloorPlanItemResponse ToFloorPlanItemResponse(this FloorPlanItem item)
  {
    return new FloorPlanItemResponse
    {
      Id = item.Id,
      Rack = item.Rack,
      Bay = item.Bay,
      Offset = item.Offset,
      Size = item.Size,
      Width = item.Width,
      Height = item.Height,
      ObjectType = item.ObjectType,
      Rotation = item.Rotation,
      Comments = item.Comments?.Select(c => c.ToFloorPlanCommentResponse()).ToList() ?? new List<FloorPlanCommentResponse>()
    };
  }

  // Mapping from FloorPlanComment to FloorPlanCommentResponse
  public static FloorPlanCommentResponse ToFloorPlanCommentResponse(this FloorPlanComments comment)
  {
    return new FloorPlanCommentResponse
    {
      Id = comment.Id,
      UserName = comment.UserName,
      Comment = comment.Comment,
      Date = comment.CreationTime.ToString("yyyy-MM-dd HH:mm:ss")
    };
  }

  public static List<DefaultSettings> ToDefaultSettings (this DefaultSettingsCommand settingCommand, Guid AssetId)
  {
    var settings = settingCommand.MapToEnumDictionary();
    var result = new List<DefaultSettings>();

    foreach(var setting in settings)
    {
      result.Add(new DefaultSettings
      {
        FloorPlanId = AssetId,
        BayConfigurationId = setting.Value,
        BayConfigCategory = setting.Key
      });
    }
    return result;
  }

  public static Dictionary<BayConfigurationCategory, Guid> MapToEnumDictionary(this DefaultSettingsCommand command)
  {
    return new Dictionary<BayConfigurationCategory, Guid>
        {
            { BayConfigurationCategory.SingleBin, command.SingleBin },
            { BayConfigurationCategory.PairSingleBin, command.PairSingleBin },
            { BayConfigurationCategory.DoubleBin, command.DoubleBin },
            { BayConfigurationCategory.PairDoubleBin, command.PairDoubleBin },
            { BayConfigurationCategory.TripleBin, command.TripleBin },
            { BayConfigurationCategory.PairTripleBin, command.PairTripleBin },
            { BayConfigurationCategory.QuadBin, command.QuadBin },
            { BayConfigurationCategory.PairQuadBin, command.PairQuadBin }
        };
  }
}
