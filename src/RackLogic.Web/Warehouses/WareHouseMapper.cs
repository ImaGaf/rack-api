using RackLogic.UseCases.Brands;
using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Brands.GetListComponentBrand;
using RackLogic.Web.Brands.GetOEMBrand;
using RackLogic.Web.Brands.GetProductsByCategory;
using RackLogic.Web.Warehouses.GetListManageUsers;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Warehouses;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class WareHouseMapper
{
  public static partial GetManageUserRespon ToGetManageUserRespon(this ManageUserDTO request);
  public static partial GetOEMBrandResponse GetGetOEMBrandRespond(this OEMBrandDTO request);
  public static partial GetListComponentBrandResponse GetComponentBrandRespond(this ListComponentBrandDTO request);
  public static partial GetProductsByCategoryResponse GetProductByCategoryRespond(this ProductDTO request);
}
