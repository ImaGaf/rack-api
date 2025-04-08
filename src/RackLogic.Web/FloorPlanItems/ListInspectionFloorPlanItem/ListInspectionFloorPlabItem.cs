using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.FloorPlanItems.Inspection;
using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlanItems.ListInspectionFloorPlanItem;

public class ListInspectionFloorPlabItem(IMediator _mediator) : Endpoint<InspectionFloorPlanItemRequest, PageResponse<InspectionFloorPlanItemDto>>
{
  public override void Configure()
  {
    Get("/floor-plan-items/listInspection");
  }

  public override async Task HandleAsync(InspectionFloorPlanItemRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new InspectionFloorPlanItemQuery(req.WarehouseId, req.pageNumber, req.pageSize), ct);

    if (result.IsSuccess)
    {
      Response = new PageResponse<InspectionFloorPlanItemDto>
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
