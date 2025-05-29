using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Inspections.GetReport;
using RackLogic.Web.Exceptions;

namespace RackLogic.Web.Inspections.GetReport;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetReport(IMediator _mediator) : Endpoint<GetReportCommand>
{
  public override void Configure()
  {
    Get("/inspections/get-report");
  }

  public override async Task HandleAsync(GetReportCommand req, CancellationToken ct)
  {
    var result = await _mediator.Send(req, ct);

    if (result.IsSuccess)
    {
      HttpContext.Response.ContentType = "text/html";
      await HttpContext.Response.WriteAsync(result.Value, ct);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
