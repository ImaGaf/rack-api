using Microsoft.AspNetCore.Mvc;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.GetListManageUsers;

public class GetManageUserRequest : PaginationRequest
{
  [FromRoute]
  public Guid WareHouseId { get; set; }
}
