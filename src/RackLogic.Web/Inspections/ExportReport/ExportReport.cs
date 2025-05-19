using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Inspections.ExportReport;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.ExportReport;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExportReport(IMediator _mediator) : Endpoint<ExportReportCommand, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/inspections/export-report");
  }

  public override async Task HandleAsync(ExportReportCommand req, CancellationToken ct)
  {
    var result = await _mediator.Send(req, ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
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
