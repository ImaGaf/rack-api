using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.DefectLogs.GetListByInspectionId;

public class GetListByInspectionIdRequest
{
  [FromRoute]
  public Guid InspectionId { get; set; }
}
