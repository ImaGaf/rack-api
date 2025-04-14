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
      BayConfigurationId = item.BayConfigurationId,
      DueDate = item.DueDate,
      Bay = item.Bay,
      FloorPlanId = item.FloorPlanId,
      Offset = item.Offset,
      Rack = item.Rack,
      Rotation = item.Rotation,
      Height = item.Height,
      InspectionId = item.InspectionId,
      Size = item.Size,
      Width = item.Width,
      ObjectType = item.ObjectType,
      BayConfiguration = item.BayConfiguration?.Mapping(),
      Inspection = item.Inspection?.Mapping(),
      DefectLogs = item.DefectLogs?.Select(x => x.Mapping()).ToList(),
    };
  }

  public static DefectLogDto Mapping(this DefectLog defectLog)
  {
    return new DefectLogDto
    {
      TicketId = defectLog.TicketId,
      ComponentId = defectLog.ComponentId,
      FloorPlanItemId = defectLog.FloorPlanItemId,
      RelatedComponentId = defectLog.RelatedComponentId,
      DrawPosition = defectLog.DrawPosition,
      ComponentIndex = defectLog.ComponentIndex,
      IsRear = defectLog.IsRear,
      DefectGrade = defectLog.DefectGrade,
      DefectPriority = defectLog.DefectPriority,
      DefectType = defectLog.DefectType,
      DefectStatus = defectLog.DefectStatus,
      CorrectiveAction = defectLog.CorrectiveAction,
      Comment = defectLog.Comment,
      Component = defectLog.Component?.Mapping(),
      RelatedComponent = defectLog.RelatedComponent?.Mapping()
    };
  }

  public static InspectionDto Mapping(this Core.Common.Entity.Inspection inspection)
  {
    return new InspectionDto
    {
      FloorPlanItemId = inspection.FloorPlanItemId,
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
