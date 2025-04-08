using System.Security.Claims;
using RackLogic.UseCases.Brands;
using RackLogic.UseCases.Warehouses;
using RackLogic.UseCases.Warehouses.CreateWarehouse;
using RackLogic.UseCases.Warehouses.Get;
using RackLogic.Web.Brands.GetAll;
using RackLogic.Web.Brands.GetOEMBrand;
using RackLogic.Web.Brands.GetProductsByCategory;
using RackLogic.Web.Warehouses.CreateWarehouse;
using RackLogic.Web.Warehouses.Get;
using RackLogic.Web.Warehouses.GetListManageUsers;
using RackLogic.Web.Warehouses.GetWarehouseFinalUse;
using RackLogic.Web.Warehouses.GetListWarehouses;
using Riok.Mapperly.Abstractions;
using RackLogic.Web.Brands.GetListProductsBrand;
using RackLogic.Web.BayConfigurator.CreateProduct;
using RackLogic.UseCases.BayConfigurator.CreateProduct;

namespace RackLogic.Web.Warehouses;

[Mapper(IncludedMembers = MemberVisibility.All, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class WareHouseMapper
{
  public static partial GetManageUserRespon ToGetManageUserRespon(this ManageUserDTO request);
  public static partial GetOEMBrandResponse GetGetOEMBrandRespond(this OEMBrandDTO request);
  public static partial GetListProductsBrandResponse GetListProductsBrandRespond(this ProductDTO request);
  public static partial GetProductsByCategoryResponse GetProductByCategoryRespond(this ProductDTO request);
  public static partial GetListWarehousesResponse GetListWarehousesResponsByUser(this WarehousesDTO request);
  public static partial CreateWarehouseCommand ToCreateWarehouseCommand(this CreateWarehouseRequest request, ClaimsPrincipal user);
  public static partial GetAllResponse GetAllBrandResponse(this GetAllBrandDTO request);
  public static partial GetWarehouseFinalUseResponse GetWarehousesFinalUseByUser(this WarehousesDTO request);
  public static partial CreateCustomComponentCommand ToCreateProductCommand(this CreateCustomComponentRequest request);
  public static partial GetProductsByCategoryResponse ToCreateProductResponse(this ProductDTO request);
  public static partial GetWarehouseByIdQuery ToGetWarehouseByIdQuery(this GetWarehouseByIdRequest request);
}
