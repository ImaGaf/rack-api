using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListWarehouses;
public record GetListWarehousesQuery( ClaimsPrincipal User) : IQuery<Result<List<WarehousesDTO>>>;

