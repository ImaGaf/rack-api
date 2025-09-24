using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.SearchReport;

public class SearchInspectionReportRequest : PaginationRequest
{
  public string? Search { get; set; }
  public Guid WarehouseId { get; set; }
}
