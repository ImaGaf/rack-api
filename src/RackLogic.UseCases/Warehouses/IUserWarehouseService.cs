using RackLogic.UseCases.Brands;

namespace RackLogic.UseCases.Warehouses;
public interface IUserWarehouseService
{
  Task<(List<ManageUserDTO> Users, int TotalRecords)> GetUsersByWarehouseIdAsync(Guid warehouseId, int pageNumber, int pageSize);
  Task<List<OEMBrandDTO>> GetAllBrandsAsync();
  Task<(List<ListComponentBrandDTO> Products, int TotalRecords)> GetProductsByBrandAsync(Guid? brandId, int pageNumber, int pageSize, bool isGetAll);
  Task<List<ProductDTO>> GetProductsByCategoryAndManufacturerAsync(Guid manufacturerId, string category);
  Task<List<WarehousesDTO>> GetWarehousesByUserIdAsync(Guid userId);
  Task<List<GetAllBrandDTO>> GetAllBrandAsync();
  Task<WarehousesDTO?> GetLastWarehouseForUserAsync(Guid userId);
  Task<bool> CheckPermissionUserRequestAsync(Guid userId, Guid warehouseId);
  Task<WarehouseProfileDTO?> GetWarehouseByIdAsync(Guid warehouseId);
}
