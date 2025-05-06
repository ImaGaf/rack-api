using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.FloorPlanItems.GetDetail;

namespace RackLogic.UseCases.Inspections.Create;
public record CreateInspectionCommand : ICommand<Result<InspectionDto>>
{
  public required Guid FloorPlanId { get; init; }
  public DateTime? NextInspectionDate { get; init; }
  public required Guid WarehouseId { get; init; }
}
