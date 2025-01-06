using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackLogic.UseCases.FloorPlans;
public class SavedFileInfo
{
  public required string FilePath { get; set; }
  public required string FileName { get; set; } 
  public required string ContentType { get; set; }
}
