using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetWarehouseFinalUse;
public record GetWarehouseFinalUseQuery(
  ClaimsPrincipal User)
  : IQuery<Result<WarehousesDTO>>;
