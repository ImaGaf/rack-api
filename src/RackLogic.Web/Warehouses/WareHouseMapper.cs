using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Warehouses.GetListComponentBrand;
using RackLogic.Web.Warehouses.GetListManageUsers;
using RackLogic.Web.Warehouses.GetOEMBrand;
using RackLogic.Web.Warehouses.GetProductsByCategory;
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
