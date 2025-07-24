using Ardalis.Result;
using RackLogic.UseCases.Brands;

namespace RackLogic.UseCases.BayConfigurator.CreateCustomComponent;
public record CreateCustomComponentCommand(
  string Category,
  string? ComponentType,
  int? Height,
  int? Width,
  int? Length,
  float? MaterialThickness,
  int? DistanceCenterToCenter,
  string? ClearEntryWidth,
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
