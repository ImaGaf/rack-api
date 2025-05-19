using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.Inspections.Delete;

public class DeleteInspectionRequest
{
  [FromRoute]
  public Guid Id { get; set; }
}
