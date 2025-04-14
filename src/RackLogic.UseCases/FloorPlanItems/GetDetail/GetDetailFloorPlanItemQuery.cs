using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.FloorPlanItems.GetDetail;

public record GetDetailFloorPlanItemQuery(Guid floorPlanItemId) : IQuery<Result<FloorPlanItemDto>>
{
}
