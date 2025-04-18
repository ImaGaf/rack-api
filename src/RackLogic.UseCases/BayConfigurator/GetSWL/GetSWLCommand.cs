using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.BayConfigurator.GetSWL;
public record GetSWLCommand(Guid Id) : ICommand<Result<SWLSignageDTO>>;
