using RackLogic.Core.Common.Entity;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Warehouses;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class WarehouseMapper
{
  public static partial WarehouseProfileDTO ToResponse(this Warehouse warehouse);
}
