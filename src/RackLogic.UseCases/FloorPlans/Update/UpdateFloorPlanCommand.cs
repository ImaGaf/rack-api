using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.FloorPlans.Update;
public record UpdateFloorPlanCommand(
  Guid UserCreatedId,
  Guid WarehouseId,
  List<UpdateFloorPlanInformationCommand> FloorAssets)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;

public record UpdateFloorPlanInformationCommand(
  Guid FloorPlanId,
  string Title,
  int Floor,
  string? Description,
  int TotalRack,
  int TotalBay,
  List<string> FileUrlsToDelete,
  List<IFormFile> Files,
  UpdateDefaultSettingsCommand UpdateDefaultSettings,
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
  Guid BayConfigurationId,
  List<UpdateFloorPlanCommentCommand>? Comments);

public record UpdateFloorPlanCommentCommand(
  Guid Id,
  string UserName,
  string Comment,
  string Date);

public record UpdateDefaultSettingsCommand(
    Guid SingleBin,
    Guid PairSingleBin,
    Guid DoubleBin,
    Guid PairDoubleBin,
    Guid TripleBin,
    Guid PairTripleBin,
    Guid QuadBin,
    Guid PairQuadBin);
