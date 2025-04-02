using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.Brands.CreateProduct;
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
    var pageSize = request.PageSize;
    var pageNumber = request.PageNumber;
    if (request.IsGetAll ?? false)
    {
      pageNumber = 1;
      pageSize = totalRecords;
    }
    return new PagedDataDTO<ListComponentBrandDTO>
    {
      PageNumber = pageNumber,
      PageSize = pageSize,
      TotalRecords = totalRecords,
      DataDTO = products
    };
  }

  public static PagedDataDTO<GetAllBrandDTO> ToPagedDataDTO(
    this GetComponentsByBrandQuery request,
    List<GetAllBrandDTO> products,
    int totalRecords)
  {
    return new PagedDataDTO<GetAllBrandDTO>
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

  public static Products Mapping(this CreateProductCommand product)
  {
    return new Products()
    {
      Category = product.Category,
      Type = product.Type,
      Height = product.Height,
      Width = product.Width,
      Length= product.Length,
      MaterialThickness = product.MaterialThickness,
      DepthOfFrame = product.DepthOfFrame,
      DistanceCenterToCenter= product.DistanceCenterToCenter,
      ClearanceBetweenFrames = product.ClearanceBetweenFrames,
      ClearEntryWidth = product.ClearEntryWidth,
      Orientation = product.Orientation,
      BeamStyle = product.BeamStyle,
      ConnectorType = product.ConnectorType,
      FinishType = product.FinishType,
      Code = double.TryParse(product.Code, out double code) ? code : 0,
      Description = product.Description,
      ManufacturerId = product.ManufacturerId,
      WarehouseId = product.WarehouseId
    };
  }

  public static ProductDTO Mapping(this Products product)
  {
    return new ProductDTO()
    {
      Id = product.Id,
      Category = product.Category,
      Type = product.Type,
      Height = product.Height,
      Width = product.Width,
      Length = product.Length,
      MaterialThickness = product.MaterialThickness,
      DepthOfFrame = product.DepthOfFrame,
      DistanceCenterToCenter = product.DistanceCenterToCenter,
      ClearanceBetweenFrames = product.ClearanceBetweenFrames,
      ClearEntryWidth = product.ClearEntryWidth,
      Orientation = product.Orientation,
      BeamStyle = product.BeamStyle,
      ConnectorType = product.ConnectorType,
      FinishType = product.FinishType,
      Code = product.Code,
      Description = product.Description,
      ManufacturerId = product.ManufacturerId,
      WarehouseId = product.WarehouseId
    };
  }

}
