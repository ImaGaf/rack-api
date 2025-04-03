namespace RackLogic.UseCases.Brands;
public class GetAllBrandDTO
{
  public OEMBrandDTO Brand { get; set; } = new();
  public List<ProductDTO> Components { get; set; } = new();
}
