using RackLogic.UseCases.Brands;
using RackLogic.UseCases.Warehouses.SearchWarehouse;
using RackLogic.UseCases.Warehouses.SearchWarehouseCertificationInspection;

namespace RackLogic.UseCases.Warehouses;
public interface IUserWarehouseService
{
  Task<(List<ManageUserDTO> Users, int TotalRecords)> GetUsersByWarehouseIdAsync(Guid warehouseId, int pageNumber, int pageSize);
  Task<List<OEMBrandDTO>> GetAllBrandsAsync();
  Task<(List<ProductDTO> Products, int TotalRecords)> GetProductsByBrandAsync(Guid? brandId, Guid? warehouseId, int pageNumber, int pageSize, bool isGetAll, string? search);
  Task<List<ProductDTO>> GetProductsByCategoryAndManufacturerAsync(Guid manufacturerId, string category);
  Task<List<WarehousesDTO>> GetWarehousesByUserIdAsync(Guid userId);
  Task<List<GetAllBrandDTO>> GetAllBrandAsync();
  Task<WarehousesDTO?> GetLastWarehouseForUserAsync(Guid userId);
  Task<bool> CheckPermissionUserRequestAsync(Guid userId, Guid warehouseId);
  Task<WarehouseProfileDTO?> GetWarehouseByIdAsync(Guid warehouseId);
  Task<OEMBrandDTO> GetBrandByComponentAsync(Guid componentId);
  Task<PagedDataDTO<WarehouseDto>> SearchWarehouseByUserIdAsync(SearchWarehouseQuery request, Guid? userId, CancellationToken cancellationToken = default);
  Task<PagedDataDTO<WarehouseCertificationInspectionDto>> SearchWarehouseCertificationInspectionAsync(
    SearchWarehouseCertificationInspectionQuery request,
    Guid? userId,
    CancellationToken cancellationToken
    );

  bool HasPermission(string entityCode, string privilegeCode);
}
