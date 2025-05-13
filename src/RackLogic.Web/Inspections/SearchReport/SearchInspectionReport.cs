using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Inspections;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.SearchReport;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SearchInspectionReport(IMediator mediator) : Endpoint<SearchInspectionReportRequest, PageResponse<InspectionReportDto>>
{
  public override void Configure()
  {
    Get("/inspections/search-report");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(SearchInspectionReportRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(req.ToQuery(), ct);

    if (result.IsSuccess)
    {
      Response = new PageResponse<InspectionReportDto>
      {
        Data = result.Value.DataDTO,
        PageNumber = result.Value.PageNumber,
        PageSize = result.Value.PageSize,
        TotalRecords = result.Value.TotalRecords
      };

      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
