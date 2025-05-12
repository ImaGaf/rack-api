using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.SearchWarehouse;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SearchWarehouse(IMediator mediator) : Endpoint<SearchWarehouseRequest, PageResponse<WarehouseDto>>
{
  public override void Configure()
  {
    Get("/warehouses/search");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(SearchWarehouseRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(req.ToQuery(), ct);

    if (result.IsSuccess)
    {
      Response = new PageResponse<WarehouseDto>
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
