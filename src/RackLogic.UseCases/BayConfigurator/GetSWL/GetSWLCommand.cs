using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.BayConfigurator.GetSWL;
public record GetSWLCommand(Guid BayConfigurationId) : ICommand<Result<SWLSignageDTO>>;
