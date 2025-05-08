namespace RackLogic.Web.Models;

public class PageResponse<T>
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public int TotalRecords { get; set; }
  public int TotalPages => CalulateTotalPage();
  public List<T> Data { get; set; } = new();

  private int CalulateTotalPage()
  {
    if (PageSize < 1) return 0;

    return (TotalRecords + PageSize - 1) / PageSize;
  }
}
