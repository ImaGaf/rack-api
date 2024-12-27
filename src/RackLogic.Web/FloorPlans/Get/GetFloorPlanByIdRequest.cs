using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.FloorPlans.Get;

public class GetFloorPlanByIdRequest
{
  [FromRoute]
  public Guid WarehouseId { get; set; }
}
