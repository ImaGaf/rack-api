namespace RackLogic.Web.FloorPlanItems.Update;

public class UpdateFloorPlanItemRequest
{
  public const string Router = "/floor-plan-items";
  public Guid Id { get; set; }
  public string? BayId { get; set; }
}
