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
      FloorPlanItems = request.Items?.Select(i => i.ToFloorPlanItem(request.FloorPlanId, bayConfiguration)).ToList() ?? new List<FloorPlanItem>(),
      DefaultSettings = request.DefaultSettings?.ToDefaultSettings(bayConfiguration, request.FloorPlanId)
    };
  }

  public static FloorPlanItem ToFloorPlanItem(this FloorPlanItemCommand item, Guid floorPlanId, IEnumerable<BayConfiguration> bayConfigurations)
  {
    var bayConfig = bayConfigurations.FirstOrDefault(b => b.BayConfigId == item.BayConfigId);

    return new FloorPlanItem
    {
      Id = item.Id,
      FloorPlanId = floorPlanId,
      BayId = item.BayId,
      Rack = item.Rack,
      Bay = item.Bay,
      Offset = item.Offset,
      Size = item.Size,
      Width = item.Width,
      Height = item.Height,
      ObjectType = item.ObjectType,
      Side = item.Side,
      Rotation = item.Rotation,
      DueDate = bayConfig?.CreatedDate.AddDays(bayConfig.InterimInterval),
      BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigId, bayConfigurations),
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
      DefaultSettings = request.DefaultSettings?.ToDefaultSettings(bayConfiguration, request.FloorPlanId)
    };
  }

  public static FloorPlanItem ToUpdateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, Guid floorPlanId, IEnumerable<BayConfiguration> bayConfigurations)
  {
    return new FloorPlanItem
    {
      Id = item.Id,
      FloorPlanId = floorPlanId,
      BayId = item.BayId,
      Rack = item.Rack,
      Bay = item.Bay,
      Offset = item.Offset,
      Size = item.Size,
      Width = item.Width,
      Height = item.Height,
      ObjectType = item.ObjectType,
      Rotation = item.Rotation,
      DueDate = item.DueDate,
      BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigId, bayConfigurations),
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
      BayId = item.BayId,
      Rack = item.Rack,
      Bay = item.Bay,
      Offset = item.Offset,
      Size = item.Size,
      Width = item.Width,
      Height = item.Height,
      ObjectType = item.ObjectType,
      Rotation = item.Rotation,
      DueDate = item.DueDate,
      BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigId, bayConfigurations),
      Comments = comments,
      Side = item.Side,
      AssetOrdinal = item.AssetOrdinal
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
        DefaultSettings: request.DefaultSettings?.ToDefaultSettingsCommand()
    );
  }

  public static DefaultSettingsCommand ToDefaultSettingsCommand(this UpdateDefaultSettingsCommand request)
  {
    return new DefaultSettingsCommand(
        SingleBin: request.SingleBin,
        SingleBinDoubleDeep: request.SingleBinDoubleDeep,
        PairSingleBin: request.PairSingleBin,
        PairSingleBinDoubleDeep: request.PairSingleBinDoubleDeep,
        DoubleBin: request.DoubleBin,
        DoubleBinDT: request.DoubleBinDT,
        DoubleBinDoubleDeep: request.DoubleBinDoubleDeep,
        PairDoubleBin: request.PairDoubleBin,
        PairDoubleBinDoubleDeep: request.PairDoubleBinDoubleDeep,
        TripleBin: request.TripleBin,
        TripleBinDT: request.TripleBinDT,
        TripleBinDoubleDeep: request.TripleBinDoubleDeep,
        PairTripleBin: request.PairTripleBin,
        PairTripleBinDoubleDeep: request.PairTripleBinDoubleDeep
    );
  }

  public static FloorPlanItemCommand ToFloorPlanItemCommand(this UpdateFloorPlanItemInformationCommand item)
  {
    return new FloorPlanItemCommand(
        Id: item.Id,
        BayId: item.BayId,
        Rack: item.Rack,
        Bay: item.Bay,
        Offset: item.Offset,
        Size: item.Size,
        Width: item.Width,
        Height: item.Height,
        ObjectType: item.ObjectType,
        AssetOrdinal: item.AssetOrdinal,
        Side: item.Side,
        Rotation: item.Rotation,
        DueDate: item.DueDate,
        BayConfigId: item.BayConfigId,
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
    floorPlan.DefaultSettings = request.DefaultSettings?.ToDefaultSettings(bayConfiguration, request.FloorPlanId);
  }

  public static void UpdateFloorPlanItem(this UpdateFloorPlanItemInformationCommand item, FloorPlanItem floorPlanItem, IEnumerable<BayConfiguration> bayConfiguration)
  {
    floorPlanItem.BayId = item.BayId;
    floorPlanItem.Rack = item.Rack;
    floorPlanItem.Bay = item.Bay;
    floorPlanItem.Offset = item.Offset;
    floorPlanItem.Size = item.Size;
    floorPlanItem.Width = item.Width;
    floorPlanItem.Height = item.Height;
    floorPlanItem.ObjectType = item.ObjectType;
    floorPlanItem.AssetOrdinal = item.AssetOrdinal;
    floorPlanItem.Side = item.Side;
    floorPlanItem.Rotation = item.Rotation;
    floorPlanItem.DueDate = item.DueDate;
    floorPlanItem.BayConfigurationId = GetBayConfigurationIdFromBayConfigId(item.BayConfigId, bayConfiguration);
    floorPlanItem.AssetOrdinal = item.AssetOrdinal;
    floorPlanItem.Side = item.Side;
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
      BayId = item.BayId,
      BayConfigId = item.BayConfiguration is not null ? item.BayConfiguration.BayConfigId : string.Empty,
      Rack = item.Rack,
      Bay = item.Bay,
      Offset = item.Offset,
      Size = item.Size,
      Width = item.Width,
      Height = item.Height,
      ObjectType = item.ObjectType,
      Side = item.Side,
      Rotation = item.Rotation,
      DueDate = item.DueDate,
      CountDefectLog = item.DefectLogs?.Count ?? 0,
      BayConfigurationId = item.BayConfigurationId,
      Comments = item.Comments?.Select(c => c.ToFloorPlanCommentResponse()).ToList() ?? new List<FloorPlanCommentResponse>(),
      AssetOrdinal = item.AssetOrdinal
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
            { BayConfigurationCategory.SingleBinDoubleDeep, command.SingleBinDoubleDeep },
            { BayConfigurationCategory.PairSingleBin, command.PairSingleBin },
            { BayConfigurationCategory.PairSingleBinDoubleDeep, command.PairSingleBinDoubleDeep },
            { BayConfigurationCategory.DoubleBin, command.DoubleBin },
            { BayConfigurationCategory.DoubleBinDT, command.DoubleBinDT },
            { BayConfigurationCategory.DoubleBinDoubleDeep, command.DoubleBinDoubleDeep },
            { BayConfigurationCategory.PairDoubleBin, command.PairDoubleBin },
            { BayConfigurationCategory.PairDoubleBinDoubleDeep, command.PairDoubleBinDoubleDeep },
            { BayConfigurationCategory.TripleBin, command.TripleBin },
            { BayConfigurationCategory.TripleBinDT, command.TripleBinDT },
            { BayConfigurationCategory.TripleBinDoubleDeep, command.TripleBinDoubleDeep },
            { BayConfigurationCategory.PairTripleBin, command.PairTripleBin },
            { BayConfigurationCategory.PairTripleBinDoubleDeep, command.PairTripleBinDoubleDeep }
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
            { BayConfigurationCategory.SingleBinDoubleDeep, command.SingleBinDoubleDeep },
            { BayConfigurationCategory.PairSingleBin, command.PairSingleBin },
            { BayConfigurationCategory.PairSingleBinDoubleDeep, command.PairSingleBinDoubleDeep },
            { BayConfigurationCategory.DoubleBin, command.DoubleBin },
            { BayConfigurationCategory.DoubleBinDT, command.DoubleBinDT },
            { BayConfigurationCategory.DoubleBinDoubleDeep, command.DoubleBinDoubleDeep },
            { BayConfigurationCategory.PairDoubleBin, command.PairDoubleBin },
            { BayConfigurationCategory.PairDoubleBinDoubleDeep, command.PairDoubleBinDoubleDeep },
            { BayConfigurationCategory.TripleBin, command.TripleBin },
            { BayConfigurationCategory.TripleBinDT, command.TripleBinDT },
            { BayConfigurationCategory.TripleBinDoubleDeep, command.TripleBinDoubleDeep },
            { BayConfigurationCategory.PairTripleBin, command.PairTripleBin },
            { BayConfigurationCategory.PairTripleBinDoubleDeep, command.PairTripleBinDoubleDeep }
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
    var defaultSettingsDict = floorPlan.DefaultSettings?
        .ToDictionary(ds => ds.BayConfigCategory, ds => ds.BayConfigurationId);

    //Dictionary map Id( BayConfigurations) -> BayConfigId
    var bayConfigDict = bayConfigurations
        .ToDictionary(bc => bc.Id, bc => bc.BayConfigId);

    // Function to get BayConfigId value from defaultSettingsDict and bayConfigDict
    string? GetMappedValue(BayConfigurationCategory category)
    {
      if (defaultSettingsDict is null) return null;

      if (!defaultSettingsDict.TryGetValue(category, out var bayConfigurationId) || bayConfigurationId == null)
        return null;

      return bayConfigDict.TryGetValue(bayConfigurationId.Value, out var bayConfigId)
          ? bayConfigId
          : null;
    }

    return new DefaultSettingsResponse
    {
      SingleBin = GetMappedValue(BayConfigurationCategory.SingleBin),
      SingleBinDoubleDeep = GetMappedValue(BayConfigurationCategory.SingleBinDoubleDeep),
      PairSingleBin = GetMappedValue(BayConfigurationCategory.PairSingleBin),
      PairSingleBinDoubleDeep = GetMappedValue(BayConfigurationCategory.PairSingleBinDoubleDeep),
      DoubleBin = GetMappedValue(BayConfigurationCategory.DoubleBin),
      DoubleBinDT = GetMappedValue(BayConfigurationCategory.DoubleBinDT),
      DoubleBinDoubleDeep = GetMappedValue(BayConfigurationCategory.DoubleBinDoubleDeep),
      PairDoubleBin = GetMappedValue(BayConfigurationCategory.PairDoubleBin),
      PairDoubleBinDoubleDeep = GetMappedValue(BayConfigurationCategory.PairDoubleBinDoubleDeep),
      TripleBin = GetMappedValue(BayConfigurationCategory.TripleBin),
      TripleBinDT = GetMappedValue(BayConfigurationCategory.TripleBinDT),
      TripleBinDoubleDeep = GetMappedValue(BayConfigurationCategory.TripleBinDoubleDeep),
      PairTripleBin = GetMappedValue(BayConfigurationCategory.PairTripleBin),
      PairTripleBinDoubleDeep = GetMappedValue(BayConfigurationCategory.PairTripleBinDoubleDeep)
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
