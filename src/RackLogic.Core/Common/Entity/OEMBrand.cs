using Ardalis.SharedKernel;

namespace RackLogic.Core.Common.Entity;
public class OEMBrand : EntityBase<Guid>, IAggregateRoot
{
  public string? Name { get; set; }
  public string? Path { get; set; }
  public string? Code { get; set; }
  public Guid? WarehouseId { get; set; }
  public virtual Warehouse? Warehouse { get; set; }
  public virtual ICollection<Products> Products { get; set; } = new List<Products>();
  public virtual ICollection<BayConfiguration> BayConfigurations { get; set; } = new List<BayConfiguration>();
}
