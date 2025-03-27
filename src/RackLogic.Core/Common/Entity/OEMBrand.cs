using Ardalis.SharedKernel;

namespace RackLogic.Core.Common.Entity;
public class OEMBrand : EntityBase<Guid>, IAggregateRoot
{
  public string? Name { get; set; }
  public string? Path { get; set; }
  public string? Code { get; set; }
  public Guid? WarehouseId { get; set; }
  public Warehouse? Warehouse { get; set; }
  public List<Products> Products { get; set; } = new List<Products>();
  public List<BayConfiguration> BayConfigurations { get; set; } = new List<BayConfiguration>();
}
