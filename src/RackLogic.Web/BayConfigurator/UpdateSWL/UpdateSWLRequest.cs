namespace RackLogic.Web.BayConfigurator.UpdateSWL;

public class UpdateSWLRequest
{
  public Guid Id { get; set; }
  public float? PalletWeight { get; set; }
  public int? BulkPalletsNumberOfLevels { get; set; }
  public float? BulkPalletsLevelMaximumLoad { get; set; }
  public int? ShelvingNumberOfLevel { get; set; }
  public float? ShelvingLevelMaximumLoad { get; set; }
  public float? ToTalBayLoad { get; set; }
  public float? FirstBeamLevelMeasurementFGL { get; set; }
  public float? SecondBeamLevelMeasurementFGL { get; set; }
}
