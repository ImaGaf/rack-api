namespace RackLogic.Core.Common.Entity;
public class OEMBrand
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Path { get; set; }
  public required ICollection<Products> Products { get; set; }
}
