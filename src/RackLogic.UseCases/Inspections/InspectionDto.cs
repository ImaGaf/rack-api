using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections;

public class InspectionDto
{
  public Guid Id { get; set; }
  public Guid FloorPlanId { get; set; }
  public Guid? AuthorisedPersonId { get; set; }
  public string? AuthorisedPersonName { get; set; }
  public DateTime InspectionDate { get; set; }
  public DateTime NextInspectionDate { get; set; }
  public Guid? WarehouseId { get; set; }
  public WarehouseDto? Warehouse { get; set; }
  public bool IsConfirmAuthorization { get; set; }
}

public class InspectionReportDto
{
  public Guid Id { get; set; }
  public Guid FloorPlanId { get; set; }
  public Guid? AuthorisedPersonId { get; set; }
  public string? AuthorisedPersonName { get; set; }
  public DateTime InspectionDate { get; set; }
  public Guid? WarehouseId { get; set; }
  public string? WarehouseName { get; set; }
  public bool IsConfirmAuthorization { get; set; }
  public List<string>? FloorPlanItemInspected { get; set; }
}
