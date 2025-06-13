using FastEndpoints;
using MediatR;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.DefectLogs.GetListByInspectionId;
using RackLogic.Web.Models;

namespace RackLogic.Web.DefectLogs.GetListByInspectionId;

public class GetListByInspectionId(IMediator _mediator) : Endpoint<GetListByInspectionIdRequest, ApiResponse<IEnumerable<DefectLogDto>>>
{
  public override void Configure()
  {
    Get("/defect-logs/inspection/{inspectionId}");
  }

  public override async Task HandleAsync(GetListByInspectionIdRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetListByInspectionIdQuery(req.InspectionId));

    if (result.IsSuccess)
    {
      Response = new ApiResponse<IEnumerable<DefectLogDto>>(result.Value);
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
