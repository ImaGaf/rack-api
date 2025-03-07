using Ardalis.Result;

namespace RackLogic.UseCases.BayConfigurator.Create;

public record CreateBayConfigurationCommand(
  BayConfigurationCommand BayConfiguration,
  Guid WarehouseId,
  Guid UserId)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;

public record BayConfigurationCommand(
    DateTime CreatedDate,
    bool IsCustomBrand,
    string BayConfigId,
    string RackingType,
    DateTime InstallationDate,
    bool IsEstimatedInstallDate,
    string AsComplianceVersion,
    string Supplier,
    string StructuralDesigner,
    string ProjectRef,
    int ToleranceGrade,
    int TotalBayLoad,
    string? Comment,
    bool IsEnableInterimInspection,
    string? InterimInterval,
    ComponentCommand UpRight,
    bool HasSplice,
    int Level,
    int Pallets,
    bool HasProtector,
    object? SpliceKits,
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
    bool? IsCustomBracingDiagonal,
    ComponentCommand RowSpacer,
    int RowSpacerLevel,
    List<int> RowSpacerLevels,
    bool? IsCustomRowSpacer,
    AccessoriesCommand Accessories,
    bool? IsCustomAccessories
  );

public record ComponentCommand(
    string ComponentType,
    int? Height,
    int? Width,
    int? Length,
    string? Code,
    string Description,
    string? Orientation,
    int? DistanceCenterToCenter,
    string? Category,
    int? Quantity,
    string? Notes,
    int? LowerHeight,
    int? UpperHeight,
    bool? IsXBrace
);

public record AccessoriesCommand(
    List<ComponentCommand>? BasePlate
);
