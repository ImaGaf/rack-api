using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.FloorPlans.Detail;

public record GetDetailFloorPlanQuery(Guid wareHouseId, Guid floorPlanId) : IQuery<Result<FloorPlanDTO>>
{
}
