using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.Inspections.Create;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Inspections;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class DefectLogMapper
{
  [MapProperty(nameof(Inspection.AuthorisePersonId), nameof(InspectionDto.AuthorisedPersonId))]
  [MapProperty(nameof(Inspection.Warehouse.Id), nameof(InspectionDto.Warehouse.WarehouseId))]
  [MapProperty(nameof(Inspection.Warehouse.CompanyName), nameof(InspectionDto.Warehouse.Name))]
  [MapProperty(nameof(Inspection.Warehouse), nameof(InspectionDto.Warehouse))]
  public static partial InspectionDto ToDto(this Inspection entity);

  [MapProperty(nameof(Inspection.AuthorisePersonId), nameof(InspectionReportDto.AuthorisedPersonId))]
  [MapProperty(nameof(Inspection.Warehouse.Id), nameof(InspectionReportDto.WarehouseId))]
  [MapProperty(nameof(Inspection.Warehouse.CompanyName), nameof(InspectionReportDto.WarehouseName))]
  public static partial InspectionReportDto ToReportDto(this Inspection entity);

  public static InspectionReportDto ToReportDtoWithFullName(this Inspection entity)
  {
    var dto = entity.ToReportDto();
    dto.AuthorisedPersonName = entity.AuthorisePerson!.FirstName + " " + entity.AuthorisePerson.LastName;
    return dto;
  }

  [MapperIgnoreTarget(nameof(Inspection.Warehouse))]
  public static partial Inspection ToEntity(this InspectionDto dto);

  public static  InspectionDto ToDto(this Inspection entity)
  {
    var target = new InspectionDto();
    target.Id = entity.Id;
    target.FloorPlanId = entity.FloorPlanId;
    target.AuthorisedPersonId = entity.AuthorisePersonId;
    target.InspectionDate = entity.InspectionDate;
    target.NextInspectionDate = entity.NextInspectionDate;
    target.WarehouseId = entity.WarehouseId;
    target.IsConfirmAuthorization = entity.IsConfirmAuthorization;
    target.FloorPlanItemInspected = entity.FloorPlanItemInspection!.Split(",").ToList();
    return target;
  }

  public static partial Inspection ToEntity(this CreateInspectionCommand command);
}
