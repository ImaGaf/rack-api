

namespace RackLogic.UseCases.Warehouses;
public class PagedDataDTO<T>
{
  public int PageNumber { get; set; } = 1;
  public int PageSize { get; set; } = 6;
  public int TotalRecords { get; set; }
  public int TotalPages => CalulateTotalPage();
  public List<T> DataDTO { get; set; } = new();
  private int CalulateTotalPage()
  {
    return (TotalRecords + PageSize - 1) / PageSize;
  }
}
