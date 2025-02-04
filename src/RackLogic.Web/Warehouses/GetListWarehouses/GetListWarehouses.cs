using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.GetListWarehouses;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.GetListWarehouses;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetListWarehouses(IMediator _mediator) : EndpointWithoutRequest<ApiResponse<List<GetListWarehousesResponse>>>
{
  public override void Configure()
  {
    Get("/manager-warehouse/listWarehouse");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {

    var result = await _mediator.Send(new GetListWarehousesQuery(User), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<List<GetListWarehousesResponse>>(result.Value.Select(WareHouseMapper.GetListWarehousesResponsByUser).ToList());
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
