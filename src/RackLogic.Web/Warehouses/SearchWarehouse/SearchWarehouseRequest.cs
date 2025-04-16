using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.SearchWarehouse;

public class SearchWarehouseRequest : PaginationRequest
{
  public string? Search { get; set; }
}
