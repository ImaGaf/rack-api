using System.Security.Claims;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.FloorPlans.Update;
public record UpdateFloorPlanCommand(
  ClaimsPrincipal User,
  Guid WarehouseId,
  List<UpdateFloorPlanInformationCommand> FloorPlans)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;

public record UpdateFloorPlanInformationCommand(
  Guid FloorPlanId,
  string AssetName,
  string Floor,
  List<string>? FileUrlsToDelete,
  List<IFormFile> NewFiles,
  List<IFormFile> FilesToUpdate);
