using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.GetWarehouseFinalUse;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.GetWarehouseFinalUse;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class GetWarehouseFinalUse(IMediator _mediator) : EndpointWithoutRequest<ApiResponse<GetWarehouseFinalUseResponse>>
{
  public override void Configure()
  {
    Get("/manager-warehouse/getWarehouseLastWorking");
  }
  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _mediator.Send(new GetWarehouseFinalUseQuery(User), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<GetWarehouseFinalUseResponse>(WareHouseMapper.GetWarehousesFinalUseByUser(result.Value));
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync();
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
  
  }
