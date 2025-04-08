using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.BayConfigurator.CreateCustomComponent;
using RackLogic.UseCases.Brands.GetListProductsBrand;
using RackLogic.UseCases.Warehouses;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Brands;

[Mapper(IncludedMembers = MemberVisibility.All, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class BrandUseCasesMapper
{
  public static PagedDataDTO<ProductDTO> ToPagedDataDTO(
    this GetProductsByBrandQuery request,
    List<ProductDTO> products,
    int totalRecords)
  {
    var pageSize = request.PageSize;
    var pageNumber = request.PageNumber;
    if (request.IsGetAll ?? false)
    {
      pageNumber = 1;
      pageSize = totalRecords;
    }
    return new PagedDataDTO<ProductDTO>
    {
      PageNumber = pageNumber,
      PageSize = pageSize,
      TotalRecords = totalRecords,
      DataDTO = products
    };
  }

  public static PagedDataDTO<GetAllBrandDTO> ToPagedDataDTO(
    this GetProductsByBrandQuery request,
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

  public static GetAllBrandDTO ToGetAllBrandDTO(this OEMBrand brand, List<Products> products)
  {
    return new GetAllBrandDTO
    {
      Brand = brand.ToOEMBrandDTO(),
      Components = products.Select(p => p.ToProductDto()).ToList()
    };
  }

  public static Products Mapping(this CreateCustomComponentCommand product)
  {
    return new Products()
    {
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
      Code = double.TryParse(product.Code, out double code) ? code : 0,
      Description = product.Description,
      ManufacturerId = product.ManufacturerId,
      WarehouseId = product.WarehouseId
    };
  }

  public static partial ProductDTO ToProductDto(this Products product);
}
