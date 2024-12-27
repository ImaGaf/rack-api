using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackLogic.Core.Common.Entity;
public class Products
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string? Category { get; set; }
  public double? Height { get; set; }
  public double? Width { get; set; }
  public double? Code { get; set; }
  public string? Description { get; set; }
  public double? Price { get; set; }
  public Guid? ManufacturerId { get; set; }
  public required OEMBrand Manufacturer { get; set; }
}
