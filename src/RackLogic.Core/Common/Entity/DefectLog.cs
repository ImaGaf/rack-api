using RackLogic.Core.Common.Enums;

namespace RackLogic.Core.Common.Entity;

public class DefectLog : BaseEntityCustom
{
  public string? TicketId { get; set; }
  public Guid ComponentId { get; set; }
  public Guid FloorPlanItemId { get; set; }
  public Guid? InspectionId { get; set; }
  public Guid? RelatedComponentId { get; set; }
  public string? DrawPosition { get; set; }
  public int ComponentIndex { get; set; }
  public bool? IsRear { get; set; }
  public bool? IsXBrace { get; set; }
  public string? ComponentType { get; set; }
  public DefectGrade? DefectGrade { get; set; }
  public DefectPriority? DefectPriority { get; set; }
  public DefectType? DefectType { get; set; }
  public CorrectiveAction? CorrectiveAction { get; set; }
  public DefectStatus? DefectStatus { get; set; }
  public string? Comment { get; set; }
  public bool IsCompleted { get; set; }
  public virtual Component? Component { get; set; }
  public virtual FloorPlanItem? FloorPlanItem { get; set; }
  public virtual Component? RelatedComponent { get; set; }
  public virtual Inspection? Inspection { get; set; }

  public virtual ICollection<DefectLogFile>? DefectLogFiles { get; set; }
  public virtual ICollection<DefectLogHistory>? DefectLogHistories { get; set; }
}
