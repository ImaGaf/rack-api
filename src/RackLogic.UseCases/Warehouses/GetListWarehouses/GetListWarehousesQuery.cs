using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListWarehouses;
public record GetListWarehousesQuery : IQuery<Result<List<WarehousesDTO>>>;

