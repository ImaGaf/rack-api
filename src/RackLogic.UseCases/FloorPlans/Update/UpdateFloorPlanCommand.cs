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
  UpdateDefaultSettingsCommand DefaultSettings,
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
  string? BayConfigurationId,
  bool? IsRequiredBayConfig,
  DateTime? DueDate,
  List<UpdateFloorPlanCommentCommand>? Comments);

public record UpdateFloorPlanCommentCommand(
  Guid Id,
  string UserName,
  string Comment,
  DateTime Date);

public record UpdateDefaultSettingsCommand(
    string? SingleBin,
    string? SingleBinDoubleDeep,
    string? PairSingleBin,
    string? PairSingleBinDoubleDeep,
    string? DoubleBin,
    string? DoubleBinDoubleDeep,
    string? PairDoubleBin,
    string? PairDoubleBinDoubleDeep,
    string? TripleBin,
    string? TripleBinDoubleDeep,
    string? PairTripleBin,
    string? PairTripleBinDoubleDeep);
