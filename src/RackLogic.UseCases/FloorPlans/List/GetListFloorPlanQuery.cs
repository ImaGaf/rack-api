using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.FloorPlans.List;

public record GetListFloorPlanQuery(Guid wareHouseId) : IQuery<Result<IEnumerable<FloorPlanSimpleDto>>>
{
}
