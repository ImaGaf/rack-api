using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListManageUsers;
public record GetListManageUsersQuery(Guid WarehouseId, int PageNumber, int PageSize) : IQuery<Result<PagedDataDTO<ManageUserDTO>>>;

