using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.Update;

public record UpdateInspectionCommand : ICommand<Result<InspectionDto>>
{
  public required Guid InspectionId { get; init; }
  public required Guid FloorPlanItemId { get; init; }
  public bool IsConfirmAuthorization { get; init; }
}
