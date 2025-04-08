using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.FloorPlanItems.Inspection;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.FloorPlanItems;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class FloorPlanItemMapper
{
  //Map for inspection item
  [MapProperty(nameof(FloorPlanItem.Id), nameof(InspectionFloorPlanItemDto.FloorPlanItemId))]
  [MapProperty(nameof(FloorPlanItem.BayConfiguration.InterimInterval), nameof(InspectionFloorPlanItemDto.Interval))]
  [MapProperty(nameof(FloorPlanItem.DueDate), nameof(InspectionFloorPlanItemDto.InspectionDueDate))]
  [MapProperty(nameof(FloorPlanItem.BayConfiguration.BayConfigId), nameof(InspectionFloorPlanItemDto.ConfigId))]
  public static partial InspectionFloorPlanItemDto ToInspectionFloorPlanItemDto(this FloorPlanItem item);
}
