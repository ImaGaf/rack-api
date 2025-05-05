using RackLogic.Core.Common.Enums;
using RackLogic.UseCases.BayConfigurator;

namespace RackLogic.UseCases.FloorPlanItems.GetDetail;

public class FloorPlanItemDto
{
  public Guid Id { get; set; }
  public required int Rack { get; set; }
  public required int Bay { get; set; }
  public required Guid FloorPlanId { get; set; }
  public Guid? BayConfigurationId { get; set; }
  public DateTime? DueDate { get; set; }
  public required string Offset { get; set; }
  public int? Size { get; set; }
  public double? Width { get; set; }
  public double? Height { get; set; }
  public string? ObjectType { get; set; }
  public required double Rotation { get; set; }
  public BayConfigurationDTO? BayConfiguration { get; set; }
}

public class InspectionDto
{
  public Guid Id { get; set; }
  public Guid FloorPlanId { get; set; }
  public Guid? AuthorisedPersonId { get; set; }
  public DateTime InspectionDate { get; set; }
  public DateTime NextInspectionDate { get; set; }
  public string? BayId { get; set; }
  public string? ClientId { get; set; }
  public string? Authorise { get; set; }
  public bool IsConfirmAuthorization { get; set; }
}
