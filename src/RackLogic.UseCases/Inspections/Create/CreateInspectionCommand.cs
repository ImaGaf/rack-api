using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.Create;
public record CreateInspectionCommand : ICommand<Result<CreateInspectionDto>>
{
  public required Guid FloorPlanId { get; init; }
  public DateTime? NextInspectionDate { get; init; }
  public required Guid WarehouseId { get; init; }
}
