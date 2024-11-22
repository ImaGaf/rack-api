namespace RackLogic.Web.Exceptions;

public class ForbiddenException : Exception
{
  public string? Error { get; set; }

  public ForbiddenException(string? errors)
  {
    Error = errors;
  }
}
