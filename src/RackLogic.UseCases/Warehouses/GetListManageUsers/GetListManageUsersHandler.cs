using Ardalis.Result;
using Ardalis.SharedKernel;


namespace RackLogic.UseCases.Warehouses.GetListManageUsers;
public class GetListManageUsersHandler(IUserWarehouseService _userWarehouse) : IQueryHandler<GetListManageUsersQuery, Result<PagedDataDTO<ManageUserDTO>>>
{
  public async Task<Result<PagedDataDTO<ManageUserDTO>>> Handle(GetListManageUsersQuery request, CancellationToken cancellationToken)
  {
    var (users, totalRecords) = await _userWarehouse.GetUsersByWarehouseIdAsync(request.WareHouseId, request.PageNumber, request.PageSize);
    if (users == null)
    {
      return Result.NotFound("No users found for the given WarehouseId.");
    }
    var pagedData = new PagedDataDTO<ManageUserDTO>
    {
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      TotalRecords = totalRecords,
      DataDTO = users
    };

    return Result.Success(pagedData);
  }
}
