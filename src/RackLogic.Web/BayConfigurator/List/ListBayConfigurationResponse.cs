using RackLogic.Web.BayConfigurator.Create;

namespace RackLogic.Web.BayConfigurator.List;

public class ListBayConfigurationResponse
{
  public Guid Id { get; set; }
  public Manufacturer? Manufacturer { get; set; }
  public DateTime CreatedDate { get; set; }
  public bool IsCustomBrand { get; set; }
  public string? BayConfigId { get; set; }
  public string? RackingType { get; set; }
  public DateTime InstallationDate { get; set; }
  public bool IsEstimatedInstallDate { get; set; }
  public string? AsComplianceVersion { get; set; }
  public string? Supplier { get; set; }
  public string? StructuralDesigner { get; set; }
  public string? ProjectRef { get; set; }
  public int ToleranceGrade { get; set; }
  public int TotalBayLoad { get; set; }
  public string? Comment { get; set; }
  public bool IsEnableInterimInspection { get; set; }
  public int? InterimInterval { get; set; }
  public ComponentResponse? Upright { get; set; }
  public bool HasSplice { get; set; }
  public int Level { get; set; }
  public int Pallets { get; set; }
  public bool HasProtector { get; set; }
  public object? SpliceKits { get; set; }
  public bool? IsCustomUpRight { get; set; }
  public IEnumerable<ComponentResponse>? Beams { get; set; }
  public bool? EnableModeAlternativeBeam { get; set; }
  public bool? EnableModeLevelSpecificBeam { get; set; }
  public int? BeamWidth { get; set; }
  public List<int?>? BeamLevels { get; set; }
  public bool? IsCustomBeam { get; set; }
  public ComponentResponse? BracingHorizontal { get; set; }
  public int BracingLevel { get; set; }
  public List<int?>? BracingHorizontalLevels { get; set; }
  public bool? IsCustomBracingHorizontal { get; set; }
  public ComponentResponse? BracingDiagonal { get; set; }
  public int BracingDiagonalLevel { get; set; }
  public List<ComponentResponse>? BracingDiagonalLevels { get; set; }
  public bool? IsCustomBracingDiagonal { get; set; }
  public int? XBraceLevel { get; set; }
  public ComponentResponse? RowSpacer { get; set; }
  public int RowSpacerLevel { get; set; }
  public List<int?>? RowSpacerLevels { get; set; }
  public bool? IsCustomRowSpacer { get; set; }
  public List<ComponentResponse>? Accessories { get; set; }
  public bool? IsCustomAccessories { get; set; }
  public int LimitedFrontBeam { get; set; }
  public float? PalletWeight { get; set; }
  public int? BulkPalletsNumberOfLevels { get; set; }
  public float? BulkPalletsLevelMaximumLoad { get; set; }
  public int? ShelvingNumberOfLevel { get; set; }
  public float? ShelvingLevelMaximumLoad { get; set; }
  public float? FirstBeamLevelMeasurementFGL { get; set; }
  public float? SecondBeamLevelMeasurementFGL { get; set; }
}

public class ComponentResponse
{
  public Guid Id { get; set; }
  public required string ComponentType { get; set; }
  public int? Height { get; set; }
  public int? Width { get; set; }
  public int? Length { get; set; }
  public string? Code { get; set; }
  public string? Description { get; set; }
  public string? Orientation { get; set; }
  public int? DistanceCenterToCenter { get; set; }
  public string? Category { get; set; }
  public int? Quantity { get; set; }
  public string? Notes { get; set; }
  public int? LowerHeight { get; set; }
  public int? UpperHeight { get; set; }
  public bool? IsXBrace { get; set; }
  public string? Kit { get; set; }
  public string? ComponentKit { get; set; }
  public string? BeamStyle { get; set; }
  public float? MaterialThickness { get; set; }
  public string? ClearEntryWidth { get; set; }
  public string? ConnectorType { get; set; }
}
