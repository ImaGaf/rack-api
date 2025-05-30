using RackLogic.Core.Common.Entity;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Warehouses;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class WarehouseMapper
{
  [MapProperty(nameof(Warehouse.Id), nameof(WarehouseProfileDTO.WarehouseId))]
  [MapProperty(nameof(Warehouse.FileImageUrl), nameof(WarehouseProfileDTO.FilePathImage))]
  [MapProperty(nameof(Warehouse.CompanyName), nameof(WarehouseProfileDTO.Name))]
  public static partial WarehouseProfileDTO ToResponse(this Warehouse warehouse);

  [MapProperty(nameof(Warehouse.Id), nameof(WarehouseDto.WarehouseId))]
  [MapProperty(nameof(Warehouse.CompanyName), nameof(WarehouseDto.Name))]
  [MapProperty(nameof(Warehouse.FileImageUrl), nameof(WarehouseDto.FilePathImage))]
  public static partial WarehouseDto ToDto(this Warehouse warehouse);

  [MapProperty(nameof(Warehouse.CompanyName), nameof(WarehouseCertificationInspectionDto.Name))]
  [MapProperty(nameof(Warehouse.Id), nameof(WarehouseDto.WarehouseId))]
  [MapProperty(nameof(Warehouse.FileImageUrl), nameof(WarehouseDto.FilePathImage))]
  public static partial WarehouseCertificationInspectionDto ToWarehouseCertificationInspectionDto(this Warehouse warehouse);
}
