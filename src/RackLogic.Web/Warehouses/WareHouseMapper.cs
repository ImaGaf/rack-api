using System.Security.Claims;
using RackLogic.UseCases.Brands;
using RackLogic.UseCases.Warehouses;
using RackLogic.UseCases.Warehouses.CreateWarehouse;
using RackLogic.Web.Brands.GetAll;
using RackLogic.Web.Brands.GetListComponentBrand;
using RackLogic.Web.Brands.GetOEMBrand;
using RackLogic.Web.Brands.GetProductsByCategory;
using RackLogic.Web.Warehouses.CreateWarehouse;
using RackLogic.Web.Warehouses.GetListManageUsers;
using RackLogic.Web.Warehouses.GetWarehouseFinalUse;
using RackLogic.Web.Warehouses.GetListWarehouses;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Warehouses;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class WareHouseMapper
{
  public static partial GetManageUserRespon ToGetManageUserRespon(this ManageUserDTO request);
  public static partial GetOEMBrandResponse GetGetOEMBrandRespond(this OEMBrandDTO request);
  public static partial GetListComponentBrandResponse GetComponentBrandRespond(this ListComponentBrandDTO request);
  public static partial GetProductsByCategoryResponse GetProductByCategoryRespond(this ProductDTO request);
  public static partial GetListWarehousesResponse GetListWarehousesResponsByUser(this WarehousesDTO request);
  public static partial CreateWarehouseCommand ToCreateWarehouseCommand(this CreateWarehouseRequest request, ClaimsPrincipal user);
  public static partial GetAllResponse GetAllBrandResponse(this GetAllBrandDTO request);
  public static partial GetWarehouseFinalUseResponse GetWarehousesFinalUseByUser(this WarehousesDTO request);
}
