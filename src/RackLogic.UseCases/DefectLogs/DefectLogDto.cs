using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.BayConfigurator;
using RackLogic.UseCases.Brands;
using RackLogic.UseCases.FloorPlanItems.GetDetail;

namespace RackLogic.UseCases.DefectLogs;

public class DefectLogDto
{
  public Guid Id { get; set; }
  public string? TicketId { get; set; }
  public Guid ComponentId { get; set; }
  public Guid FloorPlanItemId { get; set; }
  public Guid? RelatedComponentId { get; set; }
  public Guid WarehouseId { get; set; }
  public string? DrawPosition { get; set; }
  public int ComponentIndex { get; set; }
  public int Bay { get; set; }
  public int Rack { get; set; }
  public bool? IsRear { get; set; }
  public DefectGrade? DefectGrade { get; set; }
  public DefectPriority? DefectPriority { get; set; }
  public DefectType? DefectType { get; set; }
  public CorrectiveAction? CorrectiveAction { get; set; }
  public DefectStatus? DefectStatus { get; set; }
  public DateTime CreationTime { get; set; }
  public Guid? CreatorId { get; set; }
  public string? Comment { get; set; }
  public OEMBrandDTO? OEMBrand { get; set; }
  public ComponentDTO? Component { get; set; }
  public InspectionDto? Inspection { get; set; }
  public List<DefectLogFileDto>? DefectLogFiles { get; set; }
  public List<DefectLogHistoryDto>? DefectLogHistories { get; set; }
}

public class DefectLogFileDto
{
  public Guid DefectLogId { get; set; }
  public string? FileName { get; set; }
  public string? FileUrl { get; set; }
  public string? ContentType { get; set; }
}

public class DefectLogHistoryDto
{
  public Guid DefectLogId { get; set; }
  public Guid UserId { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Note { get; set; }
  public DateTime? CreationTime { get; set; }
  public DefectHistoryType Type { get; set; }
}
