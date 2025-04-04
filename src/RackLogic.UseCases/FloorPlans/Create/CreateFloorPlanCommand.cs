using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.FloorPlans.Create;
public record CreateFloorPlanCommand(
  Guid UserCreatedId,
  Guid WarehouseId,
  List<FloorPlanCommand> FloorPlans)
  : Ardalis.SharedKernel.ICommand<Result<bool>>;

public record FloorPlanCommand(
    Guid FloorPlanId,
    string Title,
    int Floor,
    string? Description,
    int TotalRack,
    int TotalBay,
    List<FloorPlanItemCommand> Items,
    List<IFormFile> Files,
    DefaultSettingsCommand DefaultSettings);

public record FloorPlanItemCommand(
    Guid Id,
    int Rack,
    int Bay,
    List<FloorPlanCommentCommand>? Comments,
    string Offset,
    int? Size,
    double? Width,
    double? Height,
    string? ObjectType,
    string? BayConfigurationId,
    bool? IsRequiredBayConfig,
    double Rotation,
    DateTime? DueDate);

public record FloorPlanCommentCommand(
    Guid Id,
    string userName,
    string comment);

public record DefaultSettingsCommand(
    string? SingleBin,
    string? PairSingleBin,
    string? DoubleBin,
    string? PairDoubleBin,
    string? TripleBin,
    string? PairTripleBin,
    string? QuadBin,
    string? PairQuadBin);

