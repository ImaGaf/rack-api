using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.FloorPlanItems.GetDetail;

namespace RackLogic.UseCases.FloorPlans.DetailDefectLog;

public record GetDetailDefectLogQuery(Guid floorPlanId) : IQuery<Result<IEnumerable<FloorPlanItemDto>>>
{ }
