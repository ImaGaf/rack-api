using Microsoft.AspNetCore.Mvc;
using RackLogic.Web.Models;

namespace RackLogic.Web.BayConfigurator.List;

public class ListBayConfigurationRequest : PaginationRequest
{
  [FromRoute]
  public Guid WarehouseId { get; set; }
}
