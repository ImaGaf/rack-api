using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.FloorPlanItems.Inspection;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.FloorPlanItems;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class FloorPlanItemMapper
{
  //Map for inspection item
  [MapProperty(nameof(FloorPlanItem.Id), nameof(InspectionFloorPlanItemDto.FloorPlanItemId))]
  public static partial InspectionFloorPlanItemDto ToInspectionFloorPlanItemDto(this FloorPlanItem item);
}
