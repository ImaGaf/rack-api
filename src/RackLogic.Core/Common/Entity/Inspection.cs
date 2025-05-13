using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;

public class Inspection : BaseEntityCustom
{
  public Guid FloorPlanId { get; set; }
  public Guid? AuthorisePersonId { get; set; }
  public DateTime InspectionDate { get; set; }
  public DateTime NextInspectionDate { get; set; }
  public Guid? WarehouseId { get; set; }
  public bool IsConfirmAuthorization { get; set; }
  public virtual FloorPlan? FloorPlan { get; set; }
  public virtual ApplicationUser? AuthorisePerson { get; set; }
  public virtual Warehouse? Warehouse { get; set; }

  public virtual ICollection<DefectLog>? DefectLogs { get; set; }
}
