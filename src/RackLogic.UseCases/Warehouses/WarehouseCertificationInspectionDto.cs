namespace RackLogic.UseCases.Warehouses;

public class WarehouseCertificationInspectionDto
{
  public Guid? WarehouseId { get; set; }
  public string? Name { get; set; }
  public string? City { get; set; }
  public DateTime? InspectionDueDate { get; set; }
}
