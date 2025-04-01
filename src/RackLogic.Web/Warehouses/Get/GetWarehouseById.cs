using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.Get;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetWarehouseById(IMediator _mediator) : Endpoint<GetWarehouseByIdRequest, ApiResponse<Result<WarehouseProfileDTO>>>
{
  public override void Configure()
  {
    Get("/warehouses/getById/{warehouseId}");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(GetWarehouseByIdRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(req.ToGetWarehouseByIdQuery(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<Result<WarehouseProfileDTO>>(result);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync();
      return;
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
