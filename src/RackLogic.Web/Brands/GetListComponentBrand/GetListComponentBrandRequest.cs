using Microsoft.AspNetCore.Mvc;
using RackLogic.Web.Models;

namespace RackLogic.Web.Brands.GetListComponentBrand;

public class GetListComponentBrandRequest : PaginationRequest
{
  [FromRoute]
  public Guid BrandId { get; set; }
}
