using Ardalis.Result;
using RackLogic.UseCases.BayConfigurator.ShareSWL;

namespace RackLogic.UseCases.BayConfigurator;

public interface IBayConfigurationService
{
  Task ExportSWLAsync(ShareSWLCommand request, CancellationToken cancellationToken = default);
}
