using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.FloorPlanItems.Update;
public record UpdateFloorPlanItemCommand : ICommand<Result<FloorPlanItemDto>>
{
  public Guid Id { get; set; }
  public required string BayId { get; set; }
}
