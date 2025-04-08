using Ardalis.Result;
using RackLogic.UseCases.Brands;

namespace RackLogic.UseCases.BayConfigurator.CreateCustomComponent;
public record CreateCustomComponentCommand(
  string Category,
  string? Type,
  int? Height,
  int? Width,
  int? Length,
  int? MaterialThickness,
  int? DepthOfFrame,
  int? DistanceCenterToCenter,
  int? ClearanceBetweenFrames,
  int? ClearEntryWidth,
  string? Orientation,
  string? BeamStyle,
  string? ConnectorType,
  string? FinishType,
  string Code,
  string Description,
  string? ReferenceCode,
  Guid? ManufacturerId,
  Guid? WarehouseId
) : Ardalis.SharedKernel.ICommand<Result<ProductDTO>>;
