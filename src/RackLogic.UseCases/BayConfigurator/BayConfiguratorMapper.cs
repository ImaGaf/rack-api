using System.ComponentModel;
using System.Text.RegularExpressions;
using Ardalis.Specification;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.BayConfigurator.Create;
using RackLogic.UseCases.BayConfigurator.CreateCustomComponent;
using RackLogic.UseCases.BayConfigurator.GetSWL;
using RackLogic.UseCases.BayConfigurator.UpdateSWL;
using RackLogic.UseCases.Common.Constants;
using Riok.Mapperly.Abstractions;
using Component = RackLogic.Core.Common.Entity.Component;

namespace RackLogic.UseCases.BayConfigurator;
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class BayConfiguratorMapper
{
  public static BayConfiguration Mapping(this BayConfigurationCommand bay, Guid warehouseId)
  {
    return new BayConfiguration()
    {
      Id = new Guid(),
      CreatedDate = DateTime.UtcNow,
      IsCustomBrand = bay.IsCustomBrand,
      WarehouseId = warehouseId,
      BayConfigId = bay.BayConfigId,
      RackingType = bay.RackingType,
      InstallationDate = bay.InstallationDate,
      IsEnableInterimInspection = bay.IsEnableInterimInspection ?? false,
      ASComplianceVersion = bay.AsComplianceVersion,
      Supplier = bay.Supplier,
      StructuralDesigner = bay.StructuralDesigner,
      ProjectRef = bay.ProjectRef,
      ToleranceGrade = bay.ToleranceGrade,
      TotalBayLoad = bay.TotalBayLoad,
      Comment = bay.Comment,
      IsEstimatedInstallDate = bay.IsEstimatedInstallDate,
      InterimInterval = bay.InterimInterval ?? BayConfigurationDefaultValue.InterimInterval,
      HasSplice = bay.HasSplice,
      Level = bay.Level,
      Pallets = bay.Pallets,
      HasProtector = bay.HasProtector,
      IsCustomUpRight = bay.IsCustomUpRight,
      IsCustomBeam = bay.IsCustomBeam,
      IsCustomBracingHorizontal = bay.IsCustomBracingHorizontal,
      IsCustomBracingDiagonal = bay.IsCustomBracingDiagonal,
      IsCustomRowSpacer = bay.IsCustomRowSpacer,
      IsCustomAccessories = bay.IsCustomAccessories,
      RowSpacerLevel = bay.RowSpacerLevel,
      BracingDiagonalLevel = bay.BracingDiagonalLevel,
      BracingLevel = bay.BracingLevel,
      BeamLevel = bay.BeamLevels.Count,
      XBraceLevel = bay.XBraceLevel
    };
  }

  public static Component Mapping(this ComponentCommand component, Guid bayConfigurationId)
  {
    return new Component()
    {
      Id = new Guid(),
      BayId = bayConfigurationId,
      ComponentType = component.ComponentType.MappingEnum(component.Orientation),
      Height = component.Height,
      Width = component.Width,
      Length = component.Length,
      Code = component.Code,
      Description = component.Description,
      Orientation = component.Orientation,
      DistanceCenterToCenter = component.DistanceCenterToCenter,
      Category = component.Category?.MappingCategoryEnum(),
      Quantity = component.Quantity,
      Notes = component.Notes,
      LowerHeight = component.LowerHeight,
      UpperHeight = component.UpperHeight,
      IsXBrace = component.IsXBrace,
      BeamStyle = component.BeamStyle,
      MaterialThickness = component.MaterialThickness,
      ClearEntryWidth = component.ClearEntryWidth,
      ConnectorType = component.ConnectorType
    };
  }

  public static BayConfigurationLevels Mapping(this int value, ComponentType levelType, Guid bayConfigurationId)
  {
    return new BayConfigurationLevels()
    {
      BayId = bayConfigurationId,
      LevelType = levelType,
      LevelValue = value
    };
  }

  public static BayConfigurationLevels Mapping(this ComponentCommand component, ComponentType levelType, Guid bayConfigurationId)
  {
    return new BayConfigurationLevels()
    {
      BayId = bayConfigurationId,
      LevelType = levelType,
      Orientation = component.Orientation,
      UpperHeight = component.UpperHeight,
      LowerHeight = component.LowerHeight,
      IsXBrace = component.IsXBrace
    };
  }

  public static BayConfigurationDTO Mapping(this BayConfiguration bayConfiguration)
  {
    return new BayConfigurationDTO()
    {
      Id = bayConfiguration.Id,
      Manufacturer = bayConfiguration.Manufacturer.Mapping(),
      CreatedDate = bayConfiguration.CreatedDate,
      IsCustomBrand = bayConfiguration.IsCustomBrand,
      BayConfigId = bayConfiguration.BayConfigId,
      RackingType = bayConfiguration.RackingType,
      InstallationDate = bayConfiguration.InstallationDate,
      IsEstimatedInstallDate = bayConfiguration.IsEstimatedInstallDate,
      AsComplianceVersion = bayConfiguration.ASComplianceVersion,
      Supplier = bayConfiguration.Supplier,
      StructuralDesigner = bayConfiguration.StructuralDesigner,
      ProjectRef = bayConfiguration.ProjectRef,
      ToleranceGrade = bayConfiguration.ToleranceGrade,
      TotalBayLoad = bayConfiguration.TotalBayLoad,
      Comment = bayConfiguration.Comment,
      IsEnableInterimInspection = bayConfiguration.IsEnableInterimInspection,
      InterimInterval = bayConfiguration.InterimInterval,
      HasSplice = bayConfiguration.HasSplice,
      Level = bayConfiguration.Level,
      Pallets = bayConfiguration.Pallets,
      HasProtector = bayConfiguration.HasProtector,
      IsCustomUpRight = bayConfiguration.IsCustomUpRight,
      IsCustomBeam = bayConfiguration.IsCustomBeam,
      IsCustomBracingHorizontal = bayConfiguration.IsCustomBracingHorizontal,
      IsCustomBracingDiagonal = bayConfiguration.IsCustomBracingDiagonal,
      IsCustomRowSpacer = bayConfiguration.IsCustomRowSpacer,
      IsCustomAccessories = bayConfiguration.IsCustomAccessories,
      RowSpacerLevel = bayConfiguration.RowSpacerLevel,
      BracingDiagonalLevel = bayConfiguration.BracingDiagonalLevel,
      BracingLevel = bayConfiguration.BracingLevel,
      XBraceLevel = bayConfiguration.XBraceLevel,
      Upright = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.Upright)?.Mapping(),
      Beam = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.Beam)?.Mapping(),
      BracingHorizontal = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.BraceHorizontal)?.Mapping(),
      BracingDiagonal = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.BraceDiagonal)?.Mapping(),
      RowSpacer = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.RowSpacer)?.Mapping(),
      SpliceKits = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.Splice)?.Mapping(),
      Accessories = bayConfiguration.Components
                .Where(c => c.Category == ComponentCategory.ACCESSORIES)
                .Select(c => c.Mapping())
                .ToList(),

      BeamLevels = bayConfiguration.BayConfigurationLevels
                .Where(l => l.LevelType == ComponentType.Beam)
                .Select(l => l.LevelValue)
                .ToList(),

      BracingHorizontalLevels = bayConfiguration.BayConfigurationLevels
                .Where(l => l.LevelType == ComponentType.BraceHorizontal)
                .Select(l => l.LevelValue)
                .ToList(),

      RowSpacerLevels = bayConfiguration.BayConfigurationLevels
                .Where(l => l.LevelType == ComponentType.RowSpacer)
                .Select(l => l.LevelValue)
                .ToList(),

      BracingDiagonalLevels = bayConfiguration.BayConfigurationLevels
                .Where(l => l.LevelType == ComponentType.BraceDiagonal)
                .Select(l => l.Mapping())
                .ToList(),

      
      SWLSignage = bayConfiguration.ToDto()
    };
  }

  public static Products Mapping(this CreateCustomComponentCommand product)
  {
    return new Products()
    {
      Category = product.Category,
      ComponentType = product.ComponentType,
      Height = product.Height,
      Width = product.Width,
      Length = product.Length,
      MaterialThickness = product.MaterialThickness,
      DistanceCenterToCenter = product.DistanceCenterToCenter,
      ClearEntryWidth = product.ClearEntryWidth,
      Orientation = product.Orientation,
      BeamStyle = product.BeamStyle,
      ConnectorType = product.ConnectorType,
      FinishType = product.FinishType,
      Code = product.Code,
      Description = product.Description,
      ManufacturerId = product.ManufacturerId,
      WarehouseId = product.WarehouseId
    };
  }

  public static ComponentDTO Mapping(this Component component)
  {
    return new ComponentDTO()
    {
      Id = component.Id,
      ComponentType = component.ComponentType.ToString(),
      Height = component.Height,
      Width = component.Width,
      Length = component.Length,
      Code = component.Code,
      Description = component.Description,
      Orientation = component.Orientation,
      DistanceCenterToCenter = component.DistanceCenterToCenter,
      Category = component.Category?.ToString(),
      Quantity = component.Quantity,
      Notes = component.Notes,
      LowerHeight = component.LowerHeight,
      UpperHeight = component.UpperHeight,
      IsXBrace = component.IsXBrace
    };
  }

  public static OEMBrand Mapping(this ManufacturerCommand manufacturer, Guid warehouseId)
  {
    return new OEMBrand()
    {
      Id = new Guid(),
      Name = manufacturer.Name,
      Code = manufacturer.Code,
      WarehouseId = warehouseId
    };
  }

  public static SWLSignage Mapping(this SWLSignage signage, UpdateSWLCommand command)
  {
    signage.PalletWeight = command.PalletWeight ?? signage.PalletWeight;
    signage.BulkPalletsNumberOfLevels = command.BulkPalletsNumberOfLevels ?? signage.BulkPalletsNumberOfLevels;
    signage.BulkPalletsLevelMaximumLoad = command.BulkPalletsLevelMaximumLoad ?? signage.BulkPalletsLevelMaximumLoad;
    signage.ShelvingNumberOfLevel = command.ShelvingNumberOfLevel ?? signage.ShelvingNumberOfLevel;
    signage.ShelvingLevelMaximumLoad = command.ShelvingLevelMaximumLoad ?? signage.ShelvingLevelMaximumLoad;
    signage.ToTalBayLoad = command.ToTalBayLoad ?? signage.ToTalBayLoad;
    signage.FirstBeamLevelMeasurementFGL = command.FirstBeamLevelMeasurementFGL ?? signage.FirstBeamLevelMeasurementFGL;
    signage.SecondBeamLevelMeasurementFGL = command.SecondBeamLevelMeasurementFGL ?? signage.SecondBeamLevelMeasurementFGL;

    return signage;
  }
  public static partial SWLSignage ToEntity(this UpdateSWLCommand request);

  [MapperIgnoreTarget(nameof(SWLSignage.Id))]
  public static partial SWLSignageDTO ToDto(SWLSignage signage);

  public static SWLSignageDTO ToDto(this BayConfiguration? config)
  {
    var dto = ToDto(config?.SWLSignage ?? new SWLSignage());
    MapFromBayConfiguration(config, dto);
    return dto;
  }

  [MapProperty(nameof(BayConfiguration.InstallationDate), nameof(SWLSignageDTO.InstallationDate))]
  [MapProperty(nameof(BayConfiguration.ASComplianceVersion), nameof(SWLSignageDTO.ASComplianceVersion))]
  [MapProperty(nameof(BayConfiguration.Supplier), nameof(SWLSignageDTO.Supplier))]
  [MapProperty(nameof(BayConfiguration.StructuralDesigner), nameof(SWLSignageDTO.StructuralDesigner))]
  [MapProperty(nameof(BayConfiguration.ProjectRef), nameof(SWLSignageDTO.ProjectRef))]
  [MapProperty(nameof(BayConfiguration.ToleranceGrade), nameof(SWLSignageDTO.ToleranceGrade))]
  [MapProperty(nameof(BayConfiguration.Manufacturer.Name), nameof(SWLSignageDTO.Manufacturer))]
  public static partial void MapFromBayConfiguration(BayConfiguration? config, SWLSignageDTO dto);

  private static ComponentDTO Mapping(this BayConfigurationLevels level)
  {
    return new ComponentDTO() 
    {
      Id = level.Id,
      ComponentType = level.LevelType.ToString(),
      Orientation = level.Orientation,
      LowerHeight = level.LowerHeight,
      UpperHeight = level.UpperHeight,
      IsXBrace = level.IsXBrace
    };
  }

  private static ManufacturerDTO? Mapping(this OEMBrand? brand)
  {
    if (brand is null) return null;

    return new ManufacturerDTO()
    {
      Id = brand.Id,
      Name = brand.Name,
      Code = brand.Code,
      Path = brand.Path,
    };
  }

  private static ComponentType MappingEnum(this string type, string? orientation)
  {
    _ = Enum.TryParse($"{type}{orientation}", true, out ComponentType enumType);

    return enumType;
  }
  private static ComponentCategory MappingCategoryEnum(this string type)
  {
    _ = Enum.TryParse(type, true, out ComponentCategory enumType);

    return enumType;
  }
}
