using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackLogic.Core.Common.Entity;
public class FloorPlanFiles : BaseEntityCustom
{
  public required Guid FloorPlanId { get; set; }
  public FloorPlan? FloorPlan { get; set; }
  public required string FileName { get; set; }
  public required string FileUrl { get; set; }
  public required string ContentType { get; set; }

}
