using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.Delete;

public record DeleteInspectionCommand(Guid id) : ICommand<Result>
{
}
