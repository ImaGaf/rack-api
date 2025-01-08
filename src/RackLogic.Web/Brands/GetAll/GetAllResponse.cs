using RackLogic.Web.Brands.GetListComponentBrand;
using RackLogic.Web.Brands.GetOEMBrand;

namespace RackLogic.Web.Brands.GetAll;

public class GetAllResponse
{
  public GetOEMBrandResponse Brand { get; set; } = new();
  public List<GetListComponentBrandResponse> Components { get; set; } = new();
}
