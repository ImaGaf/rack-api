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
  public bool IsConfirmAuthorization { get; set; }
  public int? DefectCount { get; set; }
}
