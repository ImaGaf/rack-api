namespace RackLogic.UseCases.BayConfigurator.GetSWL;
public class SWLSignageDTO
{
  public Guid Id { get; set; }
  public Guid BayConfigurationId { get; set; }
  public DateTime? InstallationDate { get; set; }
  public string? ASComplianceVersion { get; set; }
  public string? Manufacturer { get; set; }
  public string? Supplier { get; set; }
  public string? StructuralDesigner { get; set; }
  public string? ProjectRef { get; set; }
  public int ToleranceGrade { get; set; }
  public float? PalletWeight { get; set; }
  public int? BulkPalletsNumberOfLevels { get; set; }
  public float? BulkPalletsLevelMaximumLoad { get; set; }
  public int? ShelvingNumberOfLevel { get; set; }
  public float? ShelvingLevelMaximumLoad { get; set; }
  public float? ToTalBayLoad { get; set; }
  public float? FirstBeamLevelMeasurementFGL { get; set; }
  public float? SecondBeamLevelMeasurementFGL { get; set; }
}
