namespace RackLogic.Web.Exceptions;

public class NotFoundException : Exception
{
  public string? Error { get; set; }

  public NotFoundException(string? errors)
  {
    Error = errors;
  }
}
