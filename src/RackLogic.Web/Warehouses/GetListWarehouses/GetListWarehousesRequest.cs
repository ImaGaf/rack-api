using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.Warehouses.GetListWarehouses;

public class GetListWarehousesRequest
{
  [FromRoute]
  public Guid UserId { get; set; }
}
