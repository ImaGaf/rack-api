using RackLogic.Web.Brands.GetListProductsBrand;
using RackLogic.Web.Brands.GetOEMBrand;

namespace RackLogic.Web.Brands.GetAll;

public class GetAllResponse
{
  public GetOEMBrandResponse Brand { get; set; } = new();
  public List<GetListProductsBrandResponse> Components { get; set; } = new();
}
