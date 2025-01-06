using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlans.Get;
public class GetFloorPlanByIdHandler(IFileService _fileService)
  : IQueryHandler<GetFloorPlanByIdQuery, Result<List<FloorPlanDTO>>>
{
  public async Task<Result<List<FloorPlanDTO>>> Handle(GetFloorPlanByIdQuery request, CancellationToken cancellationToken)
  {
    var floorPlans = await _fileService.GetFloorPlansWithFilesByWarehouseIdAsync(request.WarehouseId);

    if (floorPlans == null)
    {
      return Result.Success(new List<FloorPlanDTO>());
    }

    return Result.Success(floorPlans);
  }
}
