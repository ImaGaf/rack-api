using System.ComponentModel;
using Ardalis.Specification;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.BayConfigurator.Create;
using Component = RackLogic.Core.Common.Entity.Component;

namespace RackLogic.UseCases.BayConfigurator;
public static class BayConfiguratorMapper
{
  public static BayConfiguration Mapping(this BayConfigurationCommand bay, Guid warehouseId)
  {
    return new BayConfiguration()
    {
      Id = new Guid(),
      CreatedDate = DateTime.Now,
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
      InterimInterval = bay.InterimInterval,
      HasSplice = bay.HasSplice,
      Level = bay.Level,
      Pallets = bay.Pallets,
      HasProtector = bay.HasProtector,
      SpliceKits = bay.SpliceKits?.ToString(),
      IsCustomUpRight = bay.IsCustomUpRight,
      IsCustomBeam = bay.IsCustomBeam,
      IsCustomBracingHorizontal = bay.IsCustomBracingHorizontal,
      IsCustomBracingDiagonal = bay.IsCustomBracingDiagonal,
      IsCustomRowSpacer = bay.IsCustomRowSpacer,
      IsCustomAccessories = bay.IsCustomAccessories,
      RowSpacerLevel = bay.RowSpacerLevel,
      BracingDiagonalLevel = bay.BracingDiagonalLevel,
      BracingLevel = bay.BracingLevel,
      BeamLevel = bay.BeamLevels.Count
    };
  }

  public static Component Mapping(this ComponentCommand component, Guid bayConfigurationId)
  {
    return new Component()
    {
      Id = new Guid(),
      BayId = bayConfigurationId,
      ComponentType = component.ComponentType.MappingEnum(),
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
      IsXBrace = component.IsXBrace
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
      SpliceKits = bayConfiguration.SpliceKits,
      IsCustomUpRight = bayConfiguration.IsCustomUpRight,
      IsCustomBeam = bayConfiguration.IsCustomBeam,
      IsCustomBracingHorizontal = bayConfiguration.IsCustomBracingHorizontal,
      IsCustomBracingDiagonal = bayConfiguration.IsCustomBracingDiagonal,
      IsCustomRowSpacer = bayConfiguration.IsCustomRowSpacer,
      IsCustomAccessories = bayConfiguration.IsCustomAccessories,
      RowSpacerLevel = bayConfiguration.RowSpacerLevel,
      BracingDiagonalLevel = bayConfiguration.BracingDiagonalLevel,
      BracingLevel = bayConfiguration.BracingLevel,

      Upright = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.Upright)?.Mapping(),
      Beam = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.Beam)?.Mapping(),
      BracingHorizontal = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.BraceHorizontal)?.Mapping(),
      BracingDiagonal = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.BraceDiagonal)?.Mapping(),
      RowSpacer = bayConfiguration.Components.FirstOrDefault(c => c.ComponentType == ComponentType.RowSpacer)?.Mapping(),

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

      Accessories = bayConfiguration.Components
                .Where(c => c.ComponentType == ComponentType.Accessories)
                .Select(c => c.Mapping())
                .ToList()
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

  private static ComponentType MappingEnum(this string type)
  {
    _ = Enum.TryParse(type, true, out ComponentType enumType);

    return enumType;
  }
  private static ComponentCategory MappingCategoryEnum(this string type)
  {
    _ = Enum.TryParse(type, true, out ComponentCategory enumType);

    return enumType;
  }
}
