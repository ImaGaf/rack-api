using Microsoft.AspNetCore.Http;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.UseCases.FloorPlans;
public static partial class FloorPlanUseCasesMapper
{
  //Map For Create New FloorPlan
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
      FloorPlanItems = request.Items.Select(i => i.ToFloorPlanItem(request.FloorPlanId)).ToList(),
      DefaultSettings = request.DefaultSettings.ToDefaultSettings(request.FloorPlanId)
    };
  }
  public static FloorPlanItem ToFloorPlanItem(this FloorPlanItemCommand item, Guid floorPlanId)
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
      Comments = item.Comments?.Select(c => c.ToFloorPlanComment(item.Id)).ToList()
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


  // Map For Create new FloorPlanUpdate
  public static FloorPlan ToUpdateFloorPlan(this UpdateFloorPlanInformationCommand request, Guid warehouseId)
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
      FloorPlanItems = request.Items.Select(i => i.ToUpdateFloorPlanItem(request.FloorPlanId)).ToList(),
      DefaultSettings = request.DefaultSettings.ToDefaultSettings(request.FloorPlanId)
    };
  }
  public static FloorPlanItem ToUpdateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, Guid floorPlanId)
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
      Comments = item.Comments?.Select(c => c.ToUpdateFloorPlanComment(item.Id)).ToList()
    };
  }
  public static FloorPlanComments ToUpdateFloorPlanComment(this UpdateFloorPlanCommentCommand comment, Guid floorPlanItemId)
  {
    return new FloorPlanComments
    {
      Id = comment.Id,
      FloorPlanItemId = floorPlanItemId,
      UserName = comment.UserName,
      Comment = comment.Comment
    };
  }


  //Create new items for update FloorPlanItems in FloorPlan
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
      CreationTime = comment.Date
    };
  }

  //Map UpdateFloorPlanInformationCommand to FloorPlanCommand 3 mappers below
  public static FloorPlanCommand ToFloorPlanCommand(this UpdateFloorPlanInformationCommand request)
  {
    return new FloorPlanCommand(
        FloorPlanId: request.FloorPlanId,
        Title: request.Title,
        Floor: request.Floor,
        Description: request.Description,
        TotalRack: request.TotalRack,
        TotalBay: request.TotalBay,
        Items: request.Items.Select(i => i.ToFloorPlanItemCommand()).ToList() ?? new List<FloorPlanItemCommand>(),
        Files: request.Files ?? new List<IFormFile>(),
        DefaultSettings: request.DefaultSettings.ToDefaultSettingsCommand()
    );
  }
  public static DefaultSettingsCommand ToDefaultSettingsCommand(this UpdateDefaultSettingsCommand request)
  {
    return new DefaultSettingsCommand(
        SingleBin: request.SingleBin,
        PairSingleBin: request.PairSingleBin,
        DoubleBin: request.DoubleBin,
        PairDoubleBin: request.PairDoubleBin,
        TripleBin: request.TripleBin,
        PairTripleBin: request.PairTripleBin,
        QuadBin: request.QuadBin,
        PairQuadBin: request.PairQuadBin
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
        IsRequiredBayConfig: item.IsRequiredBayConfig,
        Comments: new List<FloorPlanCommentCommand>()
    );
  }

  //Map FloorPlan update from request
  public static void UpdateFloorPlan(this UpdateFloorPlanInformationCommand request, FloorPlan floorPlan)
  {
    floorPlan.AssetName = request.Title;
    floorPlan.FloorNumber = request.Floor;
    floorPlan.Description = request.Description;
    floorPlan.TotalRack = request.TotalRack;
    floorPlan.TotalBay = request.TotalBay;
    floorPlan.DefaultSettings = request.DefaultSettings.ToDefaultSettings(request.FloorPlanId);
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
    floorPlanItem.BayConfigurationId = item.BayConfigurationId;
  }
  public static void UpdateFloorPlanComment(this UpdateFloorPlanCommentCommand comment, FloorPlanComments floorPlanComment)
  {
    floorPlanComment.UserName = comment.UserName;
    floorPlanComment.Comment = comment.Comment;
    floorPlanComment.CreationTime = comment.Date;
  }

  //Map FloorPlan to FloorPlanDTO
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
      Items = floorPlan.FloorPlanItems.Select(i => i.ToFloorPlanItemResponse()).ToList(),
      DefaultSettings = MapFloorPlanToDefaultSettingsResponse(floorPlan)
    };
  }

  // Map from FloorPlanItem to FloorPlanItemDTO
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
      BayConfigurationId = item.BayConfigurationId,
      Comments = item.Comments?.Select(c => c.ToFloorPlanCommentResponse()).ToList() ?? new List<FloorPlanCommentResponse>()
    };
  }

  // Map from FloorPlanComment to FloorPlanCommentDTO
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

  //ToDefaultSettings for Create
  public static List<DefaultSettings> ToDefaultSettings(this DefaultSettingsCommand settingCommand, Guid AssetId)
  {
    var settings = settingCommand.MapToEnumDictionary();
    var result = new List<DefaultSettings>();

    foreach (var setting in settings)
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

  public static Dictionary<BayConfigurationCategory, Guid?> MapToEnumDictionary(this DefaultSettingsCommand command)
  {
    return new Dictionary<BayConfigurationCategory, Guid?>
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

  //ToDefaultSettings for Update

  public static List<DefaultSettings> ToDefaultSettings(this UpdateDefaultSettingsCommand updateCommand, Guid assetId)
  {
    var settings = updateCommand.MapToEnumDictionary();
    var result = new List<DefaultSettings>();

    foreach (var setting in settings)
    {
      result.Add(new DefaultSettings
      {
        FloorPlanId = assetId,
        BayConfigurationId = setting.Value,
        BayConfigCategory = setting.Key
      });
    }

    return result;
  }

  public static Dictionary<BayConfigurationCategory, Guid?> MapToEnumDictionary(this UpdateDefaultSettingsCommand command)
  {
    return new Dictionary<BayConfigurationCategory, Guid?>
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

  public static List<FloorPlan> MapFloorPlanFilesToFloorPlan(this List<FloorPlan> floorPlans, List<FloorPlanFiles> floorPlanFiles)
  {
    foreach (var floorPlan in floorPlans)
    {
      floorPlan.FloorPlanFiles = floorPlanFiles
          .Where(fp => fp.FloorPlanId == floorPlan.Id)
          .ToList();
    }
    return floorPlans;
  }

  public static DefaultSettingsResponse MapFloorPlanToDefaultSettingsResponse(FloorPlan floorPlan)
  {
    var defaultSettingsDict = floorPlan.DefaultSettings.ToDictionary(ds => ds.BayConfigCategory, ds => ds.BayConfigurationId);

    return new DefaultSettingsResponse
    {
      SingleBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.SingleBin) ? defaultSettingsDict[BayConfigurationCategory.SingleBin] : null,
      PairSingleBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.PairSingleBin) ? defaultSettingsDict[BayConfigurationCategory.PairSingleBin] : null,
      DoubleBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.DoubleBin) ? defaultSettingsDict[BayConfigurationCategory.DoubleBin] : null,
      PairDoubleBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.PairDoubleBin) ? defaultSettingsDict[BayConfigurationCategory.PairDoubleBin] : null,
      TripleBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.TripleBin) ? defaultSettingsDict[BayConfigurationCategory.TripleBin] : null,
      PairTripleBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.PairTripleBin) ? defaultSettingsDict[BayConfigurationCategory.PairTripleBin] : null,
      QuadBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.QuadBin) ? defaultSettingsDict[BayConfigurationCategory.QuadBin] : null,
      PairQuadBin = defaultSettingsDict.ContainsKey(BayConfigurationCategory.PairQuadBin) ? defaultSettingsDict[BayConfigurationCategory.PairQuadBin] : null
    };
  }
}
