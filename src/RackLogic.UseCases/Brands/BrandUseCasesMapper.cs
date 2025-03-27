using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.Brands.GetListComponentBrand;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands;
public static partial class BrandUseCasesMapper
{
  public static PagedDataDTO<ListComponentBrandDTO> ToPagedDataDTO(
    this GetComponentsByBrandQuery request,
    List<ListComponentBrandDTO> products,
    int totalRecords)
  {
    return new PagedDataDTO<ListComponentBrandDTO>
    {
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      TotalRecords = totalRecords,
      DataDTO = products
    };
  }

  // Map from OEMBrand to OEMBrandDTO
  public static OEMBrandDTO ToOEMBrandDTO(this OEMBrand brand)
{
    return new OEMBrandDTO
    {
        Id = brand.Id,
        Name = brand.Name,
        Path = brand.Path,
        Code = brand.Code,
    };
}

  // Map from Product to ListComponentBrandDTO
  public static ListComponentBrandDTO ToListComponentBrandDTO(this Products product)
  {
    return new ListComponentBrandDTO
    {
      Id = product.Id,
      Category = product.Category,
      Height = product.Height,
      Width = product.Width,
      Length = product.Length,
      Code = product.Code,
      Description = product.Description,
      Orientation = product.Orientation,
      DistanceCenterToCenter = product.DistanceCenterToCenter,
      Type = product.Type
    };
  }

  public static GetAllBrandDTO ToGetAllBrandDTO(this OEMBrand brand, List<Products> products)
  {
    return new GetAllBrandDTO
    {
      Brand = brand.ToOEMBrandDTO(),
      Components = products.Select(p => p.ToListComponentBrandDTO()).ToList()
    };
  }

}
