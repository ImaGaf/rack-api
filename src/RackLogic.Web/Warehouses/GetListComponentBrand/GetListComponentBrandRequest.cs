using Microsoft.AspNetCore.Mvc;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.GetListComponentBrand;

public class GetListComponentBrandRequest : PaginationRequest
{
  [FromRoute]
  public Guid BrandId { get; set; }
}
