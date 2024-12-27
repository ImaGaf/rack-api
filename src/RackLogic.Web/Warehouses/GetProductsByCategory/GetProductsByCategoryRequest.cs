namespace RackLogic.Web.Warehouses.GetProductsByCategory;

public class GetProductsByCategoryRequest
{
  public Guid ManufacturerId { get; set; }
  public required string Category { get; set; }
}
