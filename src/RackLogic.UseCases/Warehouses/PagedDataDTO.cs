

namespace RackLogic.UseCases.Warehouses;
public class PagedDataDTO<T>
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public int TotalRecords { get; set; }
  public List<T> Users { get; set; } = new();
}
