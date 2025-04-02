using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.Get;

public record GetWarehouseByIdQuery : IQuery<Result<WarehouseProfileDTO>>
{
  public required Guid WarehouseId { get; set; }
}
