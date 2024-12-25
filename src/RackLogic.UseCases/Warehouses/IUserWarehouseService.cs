

namespace RackLogic.UseCases.Warehouses;
public interface IUserWarehouseService
{
  Task<(List<ManageUserDTO> Users, int TotalRecords)> GetUsersByWarehouseIdAsync(Guid warehouseId, int pageNumber, int pageSize);
}
