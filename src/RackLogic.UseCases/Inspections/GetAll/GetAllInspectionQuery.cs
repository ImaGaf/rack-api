using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.GetAll;

public record GetAllInspectionQuery(
  Guid WarehouseId
  ) : IQuery<Result<IEnumerable<InspectionDto>>>
{
}
