using Microsoft.AspNetCore.Mvc;
using RackLogic.UseCases.Common.Constants;

namespace RackLogic.Web.Models;

public class PaginationRequest
{
  [FromQuery]
  public int PageNumber { get; set; } = PaginationDefaultValue.PageNumber;

  [FromQuery]
  public int PageSize { get; set; } = PaginationDefaultValue.PageSize;

  [FromQuery]
  public string? OrderBy { get; set; }

  [FromQuery]
  public bool OrderByDesc { get; set; } = true;

  [FromQuery]
  public string? ThenBy { get; set; }

  [FromQuery]
  public bool ThenByDesc { get; set; } = true;
}
