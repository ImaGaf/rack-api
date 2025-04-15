using FastEndpoints;
using MediatR;
using RackLogic.UseCases.DefectLogs.Count;
using RackLogic.Web.Models;

namespace RackLogic.Web.DefectLogs.Count;

public class CountDefectLog(IMediator _mediator) : Endpoint<CountDefectLogRequest, ApiResponse<CountDefectLogDto>>
{
  public override void Configure()
  {
    Get("defect-logs/count");
  }

  public override async Task HandleAsync(CountDefectLogRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new CountDefectLogQuery(req.WareHouseId), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<CountDefectLogDto>(result);

      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
