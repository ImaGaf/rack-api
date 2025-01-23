using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListWarehouses;
public record GetListWarehousesQuery(Guid UserId) : IQuery<Result<List<WarehousesDTO>>>;
