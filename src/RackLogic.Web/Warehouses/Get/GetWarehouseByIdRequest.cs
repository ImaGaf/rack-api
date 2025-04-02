using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.Warehouses.Get;

public class GetWarehouseByIdRequest
{
  [FromRoute]
  public Guid WarehouseId { get; set; }
}
