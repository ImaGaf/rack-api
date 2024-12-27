using System.Security.Claims;
using Ardalis.Result;

namespace RackLogic.UseCases.FloorPlans.Create;
public record CreateFloorPlanCommand(
  ClaimsPrincipal User,
  Guid WarehouseId,
  List<FloorPlanCommand> FloorPlans)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;

public record FloorPlanCommand(string AssetName, string Floor);
