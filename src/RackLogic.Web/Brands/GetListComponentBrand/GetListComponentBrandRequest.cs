using Microsoft.AspNetCore.Mvc;
using RackLogic.Web.Models;

namespace RackLogic.Web.Brands.GetListComponentBrand;

public class GetListComponentBrandRequest : PaginationRequest
{
  [FromQuery]
  public Guid? BrandId { get; set; }
}
