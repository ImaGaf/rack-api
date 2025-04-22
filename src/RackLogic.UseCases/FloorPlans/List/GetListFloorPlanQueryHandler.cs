using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.FloorPlans.List;

public class GetListFloorPlanQueryHandler(
  IUser _user,
  IFloorPlanService _floorPlanService,
  IUserWarehouseService _userWarehouseService)
  : IQueryHandler<GetListFloorPlanQuery, Result<IEnumerable<FloorPlanSimpleDto>>>
{
  public async Task<Result<IEnumerable<FloorPlanSimpleDto>>> Handle(GetListFloorPlanQuery request, CancellationToken cancellationToken)
  {
    var isOwnWareHouse = await _userWarehouseService.CheckPermissionUserRequestAsync(_user.Id ?? Guid.Empty, request.wareHouseId);

    if (!isOwnWareHouse)
      return Result.Unauthorized();

    var floorPlans = await _floorPlanService.GetListAsync(request, cancellationToken);

    return Result.Success(floorPlans);
  }
}
