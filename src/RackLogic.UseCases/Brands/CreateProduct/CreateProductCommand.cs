using Ardalis.Result;

namespace RackLogic.UseCases.Brands.CreateProduct;
public record CreateProductCommand(
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
