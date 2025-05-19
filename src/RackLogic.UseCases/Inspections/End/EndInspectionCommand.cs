using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.End;

public record EndInspectionCommand(Guid Id, bool IsEnd) : ICommand<Result>
{
}
