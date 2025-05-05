using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.FloorPlanItems.GetDetail;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Inspections;
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  public static partial InspectionDto ToDto(this Inspection entity);
}
