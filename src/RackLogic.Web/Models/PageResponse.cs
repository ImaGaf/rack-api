namespace RackLogic.Web.Models;

public class PageResponse<T>
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public int TotalRecords { get; set; }
  public List<T> Users { get; set; } = new();
}
