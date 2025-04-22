using Microsoft.AspNetCore.Http;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.List;
using RackLogic.UseCases.FloorPlans.Update;

namespace RackLogic.UseCases.FloorPlans;

public static partial class FloorPlanUseCasesMapper
{
  //Map For Create New FloorPlan
  public static FloorPlan ToFloorPlan(this FloorPlanCommand request, Guid warehouseId, IEnumerable<BayConfiguration> bayConfiguration)
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
      FloorPlanItems = request.Items.Select(i => i.ToFloorPlanItem(request.FloorPlanId, bayConfiguration)).ToList(),
      DefaultSettings = request.DefaultSettings.ToDefaultSettings(bayConfiguration, request.FloorPlanId)
    };
  }

  public static FloorPlanItem ToFloorPlanItem(this FloorPlanItemCommand item, Guid floorPlanId, IEnumerable<BayConfiguration> bayConfigurations)
  {
    var bayConfig = bayConfigurations.FirstOrDefault(b => b.BayConfigId == item.BayConfigurationId);

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
      DueDate = bayConfig?.CreatedDate.AddDays(bayConfig.InterimInterval),
      BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigurationId, bayConfigurations),
      Comments = item.Comments?.Select(c => c.ToFloorPlanComment(item.Id)).ToList(),
      Inspection = new Inspection
      {
        Id = new Guid(),
        FloorPlanItemId = item.Id,
        InspectionDate = bayConfig?.CreatedDate.AddDays(bayConfig.InterimInterval) ?? DateTime.UtcNow,
        NextInspectionDate = bayConfig?.CreatedDate.AddDays(bayConfig.InterimInterval * 2) ?? DateTime.UtcNow,
        IsConfirmAuthorization = false
      }
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
  public static FloorPlan ToUpdateFloorPlan(this UpdateFloorPlanInformationCommand request, Guid warehouseId, IEnumerable<BayConfiguration> bayConfiguration)
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
      FloorPlanItems = request.Items.Select(i => i.ToUpdateFloorPlanItem(request.FloorPlanId, bayConfiguration)).ToList(),
      DefaultSettings = request.DefaultSettings.ToDefaultSettings(bayConfiguration, request.FloorPlanId)
    };
  }

  public static FloorPlanItem ToUpdateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, Guid floorPlanId, IEnumerable<BayConfiguration> bayConfigurations)
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
      DueDate = item.DueDate,
      BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigurationId, bayConfigurations),
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
  public static FloorPlanItem ToCreateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, Guid floorPlanId, IEnumerable<BayConfiguration> bayConfigurations)
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
      DueDate = item.DueDate,
      BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigurationId, bayConfigurations),
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
        DueDate: item.DueDate,
        BayConfigurationId: item.BayConfigurationId,
        IsRequiredBayConfig: item.IsRequiredBayConfig,
        Comments: new List<FloorPlanCommentCommand>()
    );
  }

  //Map FloorPlan update from request
  public static void UpdateFloorPlan(this UpdateFloorPlanInformationCommand request, FloorPlan floorPlan, IEnumerable<BayConfiguration> bayConfiguration)
  {
    floorPlan.AssetName = request.Title;
    floorPlan.FloorNumber = request.Floor;
    floorPlan.Description = request.Description;
    floorPlan.TotalRack = request.TotalRack;
    floorPlan.TotalBay = request.TotalBay;
    floorPlan.DefaultSettings = request.DefaultSettings.ToDefaultSettings(bayConfiguration, request.FloorPlanId);
  }

  public static void UpdateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, FloorPlanItem floorPlanItem, IEnumerable<BayConfiguration> bayConfiguration)
  {
    floorPlanItem.Rack = item.Rack;
    floorPlanItem.Bay = item.Bay;
    floorPlanItem.Offset = item.Offset;
    floorPlanItem.Size = item.Size;
    floorPlanItem.Width = item.Width;
    floorPlanItem.Height = item.Height;
    floorPlanItem.ObjectType = item.ObjectType;
    floorPlanItem.Rotation = item.Rotation;
    floorPlanItem.DueDate = item.DueDate;
    floorPlanItem.BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigurationId, bayConfiguration);
  }

  public static void UpdateFloorPlanComment(this UpdateFloorPlanCommentCommand comment, FloorPlanComments floorPlanComment)
  {
    floorPlanComment.UserName = comment.UserName;
    floorPlanComment.Comment = comment.Comment;
    floorPlanComment.CreationTime = comment.Date;
  }

  //Map FloorPlan to FloorPlanDTO
  public static FloorPlanDTO ToFloorPlanDTO(this FloorPlan floorPlan, List<BayConfiguration> bayConfigurations)
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
      Items = floorPlan.FloorPlanItems.Select(i => i.ToFloorPlanItemResponse(bayConfigurations)).ToList(),
      DefaultSettings = MapFloorPlanToDefaultSettingsResponse(floorPlan, bayConfigurations)
    };
  }

  // Map from FloorPlanItem to FloorPlanItemDTO
  public static FloorPlanItemResponse ToFloorPlanItemResponse(this FloorPlanItem item, List<BayConfiguration> bayConfigurations)
  {
    var bayConfigDict = bayConfigurations.ToDictionary(bc => bc.Id, bc => bc.BayConfigId);

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
      DueDate = item.DueDate,
      BayConfigurationId = item.BayConfigurationId.HasValue && bayConfigDict.TryGetValue(item.BayConfigurationId.Value, out var bayConfigId)
            ? bayConfigId
            : null,
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
  public static List<DefaultSettings> ToDefaultSettings(this DefaultSettingsCommand settingCommand, IEnumerable<BayConfiguration> bayConfigurations, Guid AssetId)
  {
    var settings = settingCommand.MapToEnumDictionary();
    var result = new List<DefaultSettings>();

    foreach (var setting in settings)
    {
      var bayConfigurationId = bayConfigurations.FirstOrDefault(f => f.BayConfigId.Equals(setting.Value))?.Id;
      result.Add(new DefaultSettings
      {
        FloorPlanId = AssetId,
        BayConfigurationId = bayConfigurationId,
        BayConfigCategory = setting.Key
      });
    }
    return result;
  }

  public static Dictionary<BayConfigurationCategory, string?> MapToEnumDictionary(this DefaultSettingsCommand command)
  {
    return new Dictionary<BayConfigurationCategory, string?>
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
  public static List<DefaultSettings> ToDefaultSettings(this UpdateDefaultSettingsCommand updateCommand, IEnumerable<BayConfiguration> bayConfigurations, Guid assetId)
  {
    var settings = updateCommand.MapToEnumDictionary();
    var result = new List<DefaultSettings>();

    foreach (var setting in settings)
    {
      var bayConfigurationId = bayConfigurations.FirstOrDefault(f => f.BayConfigId.Equals(setting.Value))?.Id;
      result.Add(new DefaultSettings
      {
        FloorPlanId = assetId,
        BayConfigurationId = bayConfigurationId,
        BayConfigCategory = setting.Key
      });
    }

    return result;
  }

  public static Dictionary<BayConfigurationCategory, string?> MapToEnumDictionary(this UpdateDefaultSettingsCommand command)
  {
    return new Dictionary<BayConfigurationCategory, string?>
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

  //Map FloorPlan to FloorPlanDTO
  public static DefaultSettingsResponse MapFloorPlanToDefaultSettingsResponse(FloorPlan floorPlan, List<BayConfiguration> bayConfigurations)
  {
    //Dictionary map BayConfigCategory -> BayConfigurationId
    var defaultSettingsDict = floorPlan.DefaultSettings
        .ToDictionary(ds => ds.BayConfigCategory, ds => ds.BayConfigurationId);

    //Dictionary map Id( BayConfigurations) -> BayConfigId
    var bayConfigDict = bayConfigurations
        .ToDictionary(bc => bc.Id, bc => bc.BayConfigId);

    // Function to get BayConfigId value from defaultSettingsDict and bayConfigDict
    string? GetMappedValue(BayConfigurationCategory category)
    {
      if (!defaultSettingsDict.TryGetValue(category, out var bayConfigurationId) || bayConfigurationId == null)
        return null;

      return bayConfigDict.TryGetValue(bayConfigurationId.Value, out var bayConfigId)
          ? bayConfigId
          : null;
    }

    return new DefaultSettingsResponse
    {
      SingleBin = GetMappedValue(BayConfigurationCategory.SingleBin),
      PairSingleBin = GetMappedValue(BayConfigurationCategory.PairSingleBin),
      DoubleBin = GetMappedValue(BayConfigurationCategory.DoubleBin),
      PairDoubleBin = GetMappedValue(BayConfigurationCategory.PairDoubleBin),
      TripleBin = GetMappedValue(BayConfigurationCategory.TripleBin),
      PairTripleBin = GetMappedValue(BayConfigurationCategory.PairTripleBin),
      QuadBin = GetMappedValue(BayConfigurationCategory.QuadBin),
      PairQuadBin = GetMappedValue(BayConfigurationCategory.PairQuadBin),
    };
  }

  //Get BayConfigurationId From BayConfigId
  public static Guid? GetBayConfigurationIdFromBayConfigId(string? bayConfigId, IEnumerable<BayConfiguration> bayConfigurations)
  {
    if (string.IsNullOrEmpty(bayConfigId)) return null;

    var bayConfig = bayConfigurations.FirstOrDefault(bc => bc.BayConfigId == bayConfigId);
    return bayConfig?.Id;
  }

  public static FloorPlanSimpleDto Mapping(this FloorPlan floorPlan)
  {
    return new FloorPlanSimpleDto
    {
      AssetId = floorPlan.Id,
      AssetName = floorPlan.AssetName,
      FloorNumber = floorPlan.FloorNumber,
      TotalRack = floorPlan.TotalRack,
      TotalBay = floorPlan.TotalBay
    };
  }
}
