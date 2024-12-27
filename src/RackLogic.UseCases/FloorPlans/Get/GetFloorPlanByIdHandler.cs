using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Get;
public class GetFloorPlanByIdHandler(IRepositoryWithExpression<FloorPlan> _floorPlanRepository)
  : IQueryHandler<GetFloorPlanByIdQuery, Result<IEnumerable<FloorPlanDTO>>>
{
  public async Task<Result<IEnumerable<FloorPlanDTO>>> Handle(GetFloorPlanByIdQuery request, CancellationToken cancellationToken)
  {
    var result = await _floorPlanRepository.FindAsync(x => x.WarehouseId == request.WarehouseId);

    if (!result.Any()) return Result.Invalid(new ValidationError(""));

    var resultDTO = result.Select(x => new FloorPlanDTO()
    {
      WarehouseId = x.WarehouseId,
      AssetName = x.AssetName,
      Floor = x.Floor 
    });

    return Result.Success(resultDTO);
  }
}
