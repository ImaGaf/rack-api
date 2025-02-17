using System.Security.Claims;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.FloorPlans.Update;
public record UpdateFloorPlanCommand(
  ClaimsPrincipal User,
  Guid WarehouseId,
  List<UpdateFloorPlanInformationCommand> FloorAsset)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;

public record UpdateFloorPlanInformationCommand(
  Guid FloorPlanId,
  string Title,
  int Floor,
  string? Description,
  int TotalRack,
  int TotalBay,
  List<string>? FileUrlsToDelete,
  List<IFormFile> NewFiles,
  List<IFormFile> FilesToUpdate,
  List<UpdateFloorPlanItemInformationCommand> Items);

public record UpdateFloorPlanItemInformationCommand(
  Guid Id,
  int Rack,
  int Bay,
  string Offset,
  int? Size,
  double? Width,
  double? Height,
  string? ObjectType,
  double Rotation,
  List<UpdateFloorPlanCommentCommand>? Comments);

public record UpdateFloorPlanCommentCommand(
  Guid Id,
  string UserName,
  string Comment,
  string Date);
