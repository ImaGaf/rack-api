using Ardalis.SharedKernel;

namespace RackLogic.Core.Common.Entity;
public class Products : EntityBase<Guid>, IAggregateRoot
{
  public string? Category { get; set; } // Upright, Beam, Brace, Row Spacer...
  public string? ComponentType { get; set; } // Specific type within the category (if applicable)

  // Dimensions
  public int? Height { get; set; }
  public int? Width { get; set; }
  public int? Length { get; set; } // Not explicitly listed, but may be useful
  public int? MaterialThickness { get; set; } // For Upright
  public int? DistanceCenterToCenter { get; set; } // For Brace (horizontal & diagonal)
  public string? ClearEntryWidth { get; set; } // For Beam

  // Technical specifications
  public string? Orientation { get; set; } // Not explicitly listed, but may be useful
  public string? BeamStyle { get; set; } // For Beam
  public string? ConnectorType { get; set; } // For Beam
  public string? FinishType { get; set; } // For multiple categories

  // General information
  public string? Code { get; set; }
  public string? Description { get; set; }

  // Manufacturer details
  public Guid? ManufacturerId { get; set; }
  public OEMBrand? Manufacturer { get; set; }

  public Guid? WarehouseId { get; set; }
  public Warehouse? Warehouse { get; set; }
}
