using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.BayConfigurator;

namespace RackLogic.UseCases.FloorPlanItems.GetDetail;

public class FloorPlanItemDto
{
  public required int Rack { get; set; }
  public required int Bay { get; set; }
  public required Guid FloorPlanId { get; set; }
  public Guid InspectionId { get; set; }
  public Guid? BayConfigurationId { get; set; }
  public DateTime? DueDate { get; set; }
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  public required double Rotation { get; set; }
  public BayConfigurationDTO? BayConfiguration { get; set; }
  public InspectionDto? Inspection { get; set; }
  public List<DefectLogDto>? DefectLogs { get; set; }
}

public class InspectionDto
{
  public Guid Id { get; set; }
  public Guid FloorPlanItemId { get; set; }
  public Guid AuthorisedPersonId { get; set; }
  public DateTime InspectionDate { get; set; }
  public DateTime NextInspectionDate { get; set; }
  public string? BayId { get; set; }
  public string? ClientId { get; set; }
  public string? Authorise { get; set; }
  public bool IsConfirmAuthorization { get; set; }
}

public class DefectLogDto
{
  public Guid Id { get; set; }
  public string? TicketId { get; set; }
  public Guid ComponentId { get; set; }
  public Guid FloorPlanItemId { get; set; }
  public Guid? RelatedComponentId { get; set; }
  public string? DrawPosition { get; set; }
  public int ComponentIndex { get; set; }
  public bool? IsRear { get; set; }
  public DefectGrade? DefectGrade { get; set; }
  public DefectPriority? DefectPriority { get; set; }
  public DefectType? DefectType { get; set; }
  public CorrectiveAction? CorrectiveAction { get; set; }
  public DefectStatus? DefectStatus { get; set; }
  public string? Comment { get; set; }
  public ComponentDTO? Component { get; set; }
  public ComponentDTO? RelatedComponent { get; set; }
}

