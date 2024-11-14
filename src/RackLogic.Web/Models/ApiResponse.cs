namespace RackLogic.Web.Models;

public class ApiResponse<T>
{
  public T Data { get; set; }

  public ApiResponse(T data)
  {
    Data = data;
  }
}
