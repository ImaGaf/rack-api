namespace RackLogic.UseCases.Brands;
public class ProductDTO
{
  public Guid Id { get; set; }
  public string? Category { get; set; }
  public string? ComponentType { get; set; }
  public int? Height { get; set; }
  public int? Width { get; set; }
  public int? Length { get; set; }
  public int? MaterialThickness { get; set; }
  public int? DistanceCenterToCenter { get; set; }
  public int? ClearanceBetweenFrames { get; set; }
  public string? ClearEntryWidth { get; set; }
  public string? Orientation { get; set; }
  public string? BeamStyle { get; set; }
  public string? ConnectorType { get; set; }
  public string? FinishType { get; set; }
  public string? Code { get; set; }
  public string? Description { get; set; }
  public Guid? ManufacturerId { get; set; }
  public Guid? WarehouseId { get; set; }
}
