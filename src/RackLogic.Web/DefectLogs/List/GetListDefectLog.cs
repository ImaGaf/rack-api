using FastEndpoints;
using MediatR;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.DefectLogs.List;
using RackLogic.Web.Models;

namespace RackLogic.Web.DefectLogs.List;

public class GetListDefectLog(IMediator _mediator) : Endpoint<GetListDefectLogRequest, PageResponse<DefectLogDto>>
{
  public override void Configure()
  {
    Get("/defect-logs/list");
  }

  public override async Task HandleAsync(GetListDefectLogRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetListDefectLogQuery(
      req.WarehouseId, req.AssetId, req.Rack, req.Bay, req.SortBy, req.pageNumber, req.pageSize), ct);

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
