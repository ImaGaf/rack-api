using RackLogic.UseCases.Brands;
using RackLogic.UseCases.Warehouses;
using RackLogic.UseCases.Warehouses.CreateWarehouse;
using RackLogic.UseCases.Warehouses.Get;
using RackLogic.UseCases.Warehouses.SearchWarehouse;
using RackLogic.Web.Brands.GetAll;
using RackLogic.Web.Brands.GetListProductsBrand;
using RackLogic.Web.Brands.GetOEMBrand;
using RackLogic.Web.Brands.GetProductsByCategory;
using RackLogic.Web.Warehouses.CreateWarehouse;
using RackLogic.Web.Warehouses.Get;
using RackLogic.Web.Warehouses.GetListManageUsers;
using RackLogic.Web.Warehouses.GetListWarehouses;
using RackLogic.Web.Warehouses.GetWarehouseFinalUse;
using RackLogic.Web.Warehouses.SearchWarehouse;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Warehouses;

[Mapper(IncludedMembers = MemberVisibility.All, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class WareHouseMapper
{
  public static partial GetManageUserRespon ToGetManageUserRespon(this ManageUserDTO request);

  public static partial GetOEMBrandResponse GetGetOEMBrandRespond(this OEMBrandDTO request);

  public static partial GetListProductsBrandResponse GetListProductsBrandRespond(this ProductDTO request);

  public static partial GetProductsByCategoryResponse GetProductByCategoryRespond(this ProductDTO request);

  public static partial GetListWarehousesResponse GetListWarehousesResponsByUser(this WarehousesDTO request);

  public static partial CreateWarehouseCommand ToCreateWarehouseCommand(this CreateWarehouseRequest request);

  public static partial GetAllResponse GetAllBrandResponse(this GetAllBrandDTO request);

  public static partial GetWarehouseFinalUseResponse GetWarehousesFinalUseByUser(this WarehousesDTO request);

  public static partial GetProductsByCategoryResponse ToCreateProductResponse(this ProductDTO request);

  public static partial GetWarehouseByIdQuery ToGetWarehouseByIdQuery(this GetWarehouseByIdRequest request);

  public static partial SearchWarehouseQuery ToQuery(this SearchWarehouseRequest request);

  [MapProperty(nameof(WarehouseDto.CompanyName), nameof(CreateWarehouseResponse.Name))]
  [MapProperty(nameof(WarehouseDto.FileImageUrl), nameof(CreateWarehouseResponse.FilePathImage))]
  public static partial CreateWarehouseResponse ToCreateResponse(this WarehouseDto request);
}
