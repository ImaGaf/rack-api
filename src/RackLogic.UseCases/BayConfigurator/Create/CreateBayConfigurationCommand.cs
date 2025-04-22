using Ardalis.Result;
using RackLogic.UseCases.Common.Constants;

namespace RackLogic.UseCases.BayConfigurator.Create;

public record CreateBayConfigurationCommand(
  BayConfigurationCommand BayConfiguration,
  Guid WarehouseId,
  Guid UserId)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;

public record BayConfigurationCommand(
    ManufacturerCommand Manufacturer,
    DateTime? CreatedDate,
    bool IsCustomBrand,
    string BayConfigId,
    string RackingType,
    DateTime? InstallationDate,
    bool IsEstimatedInstallDate,
    string AsComplianceVersion,
    string Supplier,
    string StructuralDesigner,
    string ProjectRef,
    int ToleranceGrade,
    int TotalBayLoad,
    string? Comment,
    bool? IsEnableInterimInspection,
    ComponentCommand UpRight,
    bool? HasSplice,
    int Level,
    int Pallets,
    bool HasProtector,
    ComponentCommand? SpliceKits,
    bool? IsCustomUpRight,
    ComponentCommand Beam,
    List<int> BeamLevels,
    bool? IsCustomBeam,
    ComponentCommand BracingHorizontal,
    int BracingLevel,
    List<int> BracingHorizontalLevels,
    bool? IsCustomBracingHorizontal,
    ComponentCommand BracingDiagonal,
    int BracingDiagonalLevel,
    List<ComponentCommand> BracingDiagonalLevels,
    int? XBraceLevel,
    bool? IsCustomBracingDiagonal,
    ComponentCommand RowSpacer,
    int RowSpacerLevel,
    List<int> RowSpacerLevels,
    bool? IsCustomRowSpacer,
    List<ComponentCommand>? Accessories,
    bool? IsCustomAccessories,
    string? InterimInterval
  );

public record ComponentCommand(
    string ComponentType,
    int? Height,
    int? Width,
    int? Length,
    string? Code,
    string Description,
    string? Orientation,
    double? DistanceCenterToCenter,
    string? Category,
    int? Quantity,
    string? Notes,
    int? LowerHeight,
    int? UpperHeight,
    bool? IsXBrace,
    int? Kit,
    string? ComponentKit,
    string? BeamStyle,
    int? MaterialThickness,
    string? ClearEntryWidth,
    int? ClearanceBetweenFrames,
    string? ConnectorType
);

public record ManufacturerCommand(
  Guid? Id,
  string Name,
  string? Code,
  string? Path
);
