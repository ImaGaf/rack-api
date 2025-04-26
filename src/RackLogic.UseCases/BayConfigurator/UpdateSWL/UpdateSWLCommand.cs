using Ardalis.Result;

namespace RackLogic.UseCases.BayConfigurator.UpdateSWL;
public record UpdateSWLCommand(
  Guid Id,
  float? PalletWeight,
  int? BulkPalletsNumberOfLevels,
  float? BulkPalletsLevelMaximumLoad,
  int? ShelvingNumberOfLevel,
  float? ShelvingLevelMaximumLoad,
  float? ToTalBayLoad,
  float? FirstBeamLevelMeasurementFGL,
  float? SecondBeamLevelMeasurementFGL)
  : Ardalis.SharedKernel.ICommand<Result<BayConfigurationDTO>>;
