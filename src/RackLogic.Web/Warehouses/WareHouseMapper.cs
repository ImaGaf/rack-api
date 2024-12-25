using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Warehouses.GetListManageUsers;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Warehouses;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class WareHouseMapper
{
  public static partial GetManageUserRespon ToGetManageUserRespon(this ManageUserDTO request);
}
