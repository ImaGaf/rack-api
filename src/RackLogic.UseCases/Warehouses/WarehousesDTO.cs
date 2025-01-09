using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackLogic.UseCases.Warehouses;
public class WarehousesDTO
{
  public Guid WarahouseId { get; set; }
  public string? Name{ get; set; }
  public string? FilePathImage { get; set; }
}
