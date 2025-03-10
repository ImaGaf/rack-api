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
    List<IFormFile> Files);

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
    double Rotation);

public record FloorPlanCommentCommand(
    Guid Id,
    string userName,
    string comment);


