using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.Warehouses.WarehouseFinalUse;
public class WarehouseFinalUseHandler(
  IRepositoryWithExpression<WarehouseFinalUses> _warehouseFinalUseRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository)
  : ICommandHandler<WarehouseFinalUseCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(WarehouseFinalUseCommand request, CancellationToken cancellationToken)
  {
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();
    var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null) return Result.Invalid(new ValidationError("User not found."));
    var userId = Guid.Parse(userIdClaim.Value);

    // Check if user has permission to work in this warehouse
    var userHasAccess = await _userWarehouseRoleRepository
        .FirstOrDefaultAsync(uwr => uwr.UserId == userId && uwr.WarehouseId == request.WarehouseId, cancellationToken)
        != null;

    if (!userHasAccess) return Result.Forbidden("User does not have permission to access this warehouse.");

    // Check if the user has any record
    var existingRecord = await _warehouseFinalUseRepository
        .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

    if (existingRecord != null) await _warehouseFinalUseRepository.DeleteAsync(existingRecord, cancellationToken);

    var newWarehouseFinalUse = new WarehouseFinalUses
    {
      UserId = userId,
      WarehouseId = request.WarehouseId
    };
    await _warehouseFinalUseRepository.AddAsync(newWarehouseFinalUse, cancellationToken);
    return Result.Success(true);
  }
}
