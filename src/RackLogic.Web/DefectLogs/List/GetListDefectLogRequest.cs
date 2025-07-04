using RackLogic.Core.Common.Enums;
using RackLogic.Web.Models;

namespace RackLogic.Web.DefectLogs.List;

public class GetListDefectLogRequest : PaginationRequest
{
  public Guid WarehouseId { get; set; }
  public Guid? AssetId { get; set; }
  public bool IsCompleted { get; set; }
  public int? Rack { get; set; }
  public int? Bay { get; set; }
  public SortBy SortBy { get; set; }
  public DefectStatus? DefectStatus { get; set; }
  public DefectPriority? DefectPriority { get; set; }
}
