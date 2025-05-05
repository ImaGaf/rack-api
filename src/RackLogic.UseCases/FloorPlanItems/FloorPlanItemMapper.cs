using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.BayConfigurator;
using RackLogic.UseCases.FloorPlanItems.GetDetail;
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

  public static FloorPlanItemDto Mapping(this FloorPlanItem item)
  {
    return new FloorPlanItemDto
    {
      Id = item.Id,
      BayConfigurationId = item.BayConfigurationId,
      DueDate = item.DueDate,
      Bay = item.Bay,
      FloorPlanId = item.FloorPlanId,
      Offset = item.Offset,
      Rack = item.Rack,
      Rotation = item.Rotation,
      Height = item.Height,
      Size = item.Size,
      Width = item.Width,
      ObjectType = item.ObjectType,
      BayConfiguration = item.BayConfiguration?.Mapping(),
    };
  }

  public static InspectionDto Mapping(this Core.Common.Entity.Inspection inspection)
  {
    return new InspectionDto
    {
      FloorPlanId = inspection.FloorPlanId,
      AuthorisedPersonId = inspection.AuthorisePersonId,
      InspectionDate = inspection.InspectionDate,
      NextInspectionDate = inspection.NextInspectionDate,
      BayId = inspection.BayId,
      ClientId = inspection.ClientId,
      Authorise = inspection.Authorise,
      IsConfirmAuthorization = inspection.IsConfirmAuthorization,
    };
  }
}
