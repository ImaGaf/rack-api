namespace RackLogic.Web.Exceptions;

public class InvalidException : Exception
{
  public List<string>? Errors { get; set; }

  public InvalidException(List<string>? errors)
  {
    Errors = errors;
  }
}
