using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.BayConfigurator.ShareSWL;

public record ShareSWLCommand(
  Guid BayConfigurationId,
  string? Message,
  string? AssetName,
  string? RackNumber,
  string Email
  ) : ICommand<Result>
{
}
