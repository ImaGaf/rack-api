using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.SearchWarehouseCertificationInspection;

public class SearchWarehouseCertificationInspectionRequest : PaginationRequest
{
  public string? Search { get; set; }
}
