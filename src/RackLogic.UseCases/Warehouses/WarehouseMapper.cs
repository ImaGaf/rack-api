using RackLogic.Core.Common.Entity;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Warehouses;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class WarehouseMapper
{
  public static partial WarehouseProfileDTO ToResponse(this Warehouse warehouse);

  [MapProperty(nameof(Warehouse.Id), nameof(WarehouseDto.WarehouseId))]
  public static partial WarehouseDto ToDto(this Warehouse warehouse);
}
