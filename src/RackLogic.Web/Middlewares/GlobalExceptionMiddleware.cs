using RackLogic.Web.Exceptions;

namespace RackLogic.Web.Middlewares;

public class GlobalExceptionMiddleware
{
  private readonly RequestDelegate _next;

  public GlobalExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (InvalidException ex)
    {
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      await context.Response.WriteAsJsonAsync(new
      {
        StatusCode = StatusCodes.Status400BadRequest,
        Message = "One or more validation errors occurred.",
        Errors = ex.Errors
      });
    }
    catch (ForbiddenException ex)
    {
      context.Response.StatusCode = StatusCodes.Status403Forbidden;
      await context.Response.WriteAsJsonAsync(new
      {
        StatusCode = StatusCodes.Status403Forbidden,
        Message = "One or more validation errors occurred.",
        Errors = ex.Error
      });
    }
    catch (Exception ex)
    {
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await context.Response.WriteAsJsonAsync(new
      {
        StatusCode = StatusCodes.Status500InternalServerError,
        Message = "An unexpected error occurred.",
        Details = ex.Message
      });
    }
  }
}
