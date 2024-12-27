using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.FloorPlans.Get;
public record GetFloorPlanByIdQuery(Guid WarehouseId) : IQuery<Result<IEnumerable<FloorPlanDTO>>>;
