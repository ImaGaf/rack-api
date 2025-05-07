using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.Inspections.Get;

public class GetInspectionByIdRequest
{
  [FromRoute]
  public Guid InspectionId { get; set; }
}
