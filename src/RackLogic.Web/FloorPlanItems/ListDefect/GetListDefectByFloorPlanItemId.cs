using FastEndpoints;
using MediatR;
using RackLogic.UseCases.DefectLogs.List;
using RackLogic.UseCases.DefectLogs;
using RackLogic.Web.DefectLogs.List;
using RackLogic.Web.Models;
using RackLogic.UseCases.FloorPlanItems.ListDefect;

namespace RackLogic.Web.FloorPlanItems.ListDefect;

public class GetListDefectByFloorPlanItemId(IMediator _mediator) : Endpoint<GetListDefectByFloorPlanItemIdRequest, PageResponse<DefectLogDto>>
{
  public override void Configure()
  {
    Get("/floor-plan-items/defects");
  }

  public override async Task HandleAsync(GetListDefectByFloorPlanItemIdRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetListDefectLogByFloorPlanItemIdQuery(req.FloorPlanItemId, req.PageNumber, req.PageSize), ct);

    if (result.IsSuccess)
    {
      Response = new PageResponse<DefectLogDto>
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
