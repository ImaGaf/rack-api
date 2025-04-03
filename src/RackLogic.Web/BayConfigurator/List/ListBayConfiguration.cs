using FastEndpoints;
using MediatR;
using RackLogic.UseCases.BayConfigurator.List;
using RackLogic.Web.Models;

namespace RackLogic.Web.BayConfigurator.List;

public class ListBayConfiguration(IMediator _mediator) : Endpoint<ListBayConfigurationRequest, PageResponse<ListBayConfigurationResponse>>
{
  public override void Configure()
  {
    Get("/bay-configurators/{warehouseId}");
  }

  public override async Task HandleAsync(ListBayConfigurationRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(
      new ListBayConfiguratorQuery(
        req.WarehouseId,
        req.PageNumber,
        req.PageSize),
      ct);

    if (result.IsSuccess)
    {
      var rs =
      Response = new PageResponse<ListBayConfigurationResponse>
      {
        PageNumber = result.Value.PageNumber,
        PageSize = result.Value.PageSize,
        TotalRecords = result.Value.TotalRecords,
        Data = result.Value.DataDTO.Select(x => x.ToBayConfigurationResponse()).ToList() ?? new List<ListBayConfigurationResponse>()
      };
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
