using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.BayConfigurator.ShareSWL;

public record ShareSWLCommand(
  Guid BayConfigurationId,
  string? Message,
  Guid AssetId,
  string? RackNumber,
  string Email
  ) : ICommand<Result>
{
}
