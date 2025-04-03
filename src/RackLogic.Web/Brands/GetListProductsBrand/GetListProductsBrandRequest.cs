using Microsoft.AspNetCore.Mvc;
using RackLogic.Web.Models;

namespace RackLogic.Web.Brands.GetListProductsBrand;

public class GetListProductsBrandRequest : PaginationRequest
{
  [FromQuery]
  public Guid? BrandId { get; set; }
  public Guid? WarehouseId { get; set; }
  public bool? IsGetAll { get; set; }
}
