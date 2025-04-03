using Microsoft.AspNetCore.Mvc;
using RackLogic.UseCases.Common.Constants;

namespace RackLogic.Web.Models;

public class PaginationRequest
{
  [FromQuery]
  public int PageNumber { get; set; } = PaginationDefaultValue.PageNumber;

  [FromQuery]
  public int PageSize { get; set; } = PaginationDefaultValue.PageSize;
}
