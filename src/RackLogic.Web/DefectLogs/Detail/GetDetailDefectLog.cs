using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.DefectLogs.Detail;
using RackLogic.Web.Models;

namespace RackLogic.Web.DefectLogs.Detail;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetDetailDefectLog(IMediator _mediator) : Endpoint<GetDetailDefectLogRequest, ApiResponse<DefectLogDto>>
{
  public override void Configure()
  {
    Get("defect-logs/detail");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(GetDetailDefectLogRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetDetailDefectLogQuery(req.DefectLogId), ct);

    if (result.IsSuccess)
      Response = new ApiResponse<DefectLogDto>(result.Value);

    if (result.Status == ResultStatus.NotFound)
      await SendNotFoundAsync(ct);

    return;
  }
}
