using RackLogic.UseCases.BayConfigurator.GetSWL;
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
  public string? InterimInterval { get; set; }
  public ComponentResponse? Upright { get; set; }
  public bool HasSplice { get; set; }
  public int Level { get; set; }
  public int Pallets { get; set; }
  public bool HasProtector { get; set; }
  public object? SpliceKits { get; set; }
  public bool? IsCustomUpRight { get; set; }
  public ComponentResponse? Beam { get; set; }
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

  public SWLSignageDTO? SWLSignage { get; set; }
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
  public int? Kit { get; set; }
  public string? ComponentKit { get; set; }
  public string? BeamStyle { get; set; }
  public int? MaterialThickness { get; set; }
  public string? ClearEntryWidth { get; set; }
  public int? ClearanceBetweenFrames { get; set; }
  public string? ConnectorType { get; set; }
}
