namespace RackLogic.Core.Common.Entity;
public class SWLSignage : BaseEntityCustom
{
  public Guid BayConfigurationId { get; set; }
  public float? PalletWeight { get; set; }
  public int? BulkPalletsNumberOfLevels  { get; set; }
  public float? BulkPalletsLevelMaximumLoad { get; set; }
  public int? ShelvingNumberOfLevel { get; set; }
  public float? ShelvingLevelMaximumLoad { get; set; }
  public float? ToTalBayLoad { get; set; }
  public float? FirstBeamLevelMeasurementFGL { get; set; }
  public float? SecondBeamLevelMeasurementFGL { get; set; }
  public virtual BayConfiguration? BayConfiguration { get; set; }
}
