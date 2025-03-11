namespace RackLogic.UseCases.BayConfigurator;

public class BayConfigurationDTO
{
  public Guid Id { get; set; }
  public DateTime CreatedDate { get; set; }
  public bool IsCustomBrand { get; set; }
  public required string BayConfigId { get; set; }
  public required string RackingType { get; set; }
  public DateTime? InstallationDate { get; set; }
  public bool IsEstimatedInstallDate { get; set; }
  public required string AsComplianceVersion { get; set; }
  public required string Supplier { get; set; }
  public required string StructuralDesigner { get; set; }
  public required string ProjectRef { get; set; }
  public int ToleranceGrade { get; set; }
  public int TotalBayLoad { get; set; }
  public string? Comment { get; set; }
  public bool IsEnableInterimInspection { get; set; }
  public string? InterimInterval { get; set; }
  public ComponentDTO? Upright { get; set; }
  public bool? HasSplice { get; set; }
  public int Level { get; set; }
  public int Pallets { get; set; }
  public bool HasProtector { get; set; }
  public object? SpliceKits { get; set; }
  public bool? IsCustomUpRight { get; set; }
  public ComponentDTO? Beam { get; set; }
  public List<int?>? BeamLevels { get; set; }
  public bool? IsCustomBeam { get; set; }
  public ComponentDTO? BracingHorizontal { get; set; }
  public int BracingLevel { get; set; }
  public List<int?>? BracingHorizontalLevels { get; set; }
  public bool? IsCustomBracingHorizontal { get; set; }
  public ComponentDTO? BracingDiagonal { get; set; }
  public int BracingDiagonalLevel { get; set; }
  public List<ComponentDTO>? BracingDiagonalLevels { get; set; }
  public bool? IsCustomBracingDiagonal { get; set; }
  public ComponentDTO? RowSpacer { get; set; }
  public int RowSpacerLevel { get; set; }
  public List<int?>? RowSpacerLevels { get; set; }
  public bool? IsCustomRowSpacer { get; set; }
  public List<ComponentDTO>? Accessories { get; set; }
  public bool? IsCustomAccessories { get; set; }
}

public class ComponentDTO
{
  public Guid Id { get; set; }
  public required string ComponentType { get; set; }
  public int? Height { get; set; }
  public int? Width { get; set; }
  public int? Length { get; set; }
  public string? Code { get; set; }
  public string? Description { get; set; }
  public string? Orientation { get; set; }
  public double? DistanceCenterToCenter { get; set; }
  public string? Category { get; set; }
  public int? Quantity { get; set; }
  public string? Notes { get; set; }
  public int? LowerHeight { get; set; }
  public int? UpperHeight { get; set; }
  public bool? IsXBrace { get; set; }
}
