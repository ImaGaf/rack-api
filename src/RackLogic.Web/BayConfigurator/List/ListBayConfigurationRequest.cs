using Microsoft.AspNetCore.Mvc;
using RackLogic.Web.Models;

namespace RackLogic.Web.BayConfigurator.List;

public class ListBayConfigurationRequest : PaginationRequest
{
  [FromRoute]
  public Guid WarehouseId { get; set; }
  [FromRoute]
  public bool? IsGetAll { get; set; }
  [FromRoute]
  public Guid? AssetId { get; set; }
  [FromRoute]
  public string? BayConfigId { get; set; }
}
