using Microsoft.AspNetCore.Mvc;

namespace RackLogic.Web.Models;

public class PaginationRequest
{
  [FromQuery]
  public int? PageNumber { get; set; } = 0;

  [FromQuery]
  public int? PageSize { get; set; } = 0;
}
