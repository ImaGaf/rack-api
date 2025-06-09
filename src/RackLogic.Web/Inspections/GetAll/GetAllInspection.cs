using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Inspections.GetAll;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.GetAll;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetAllInspection(IMediator _mediator) : Endpoint<GetAllInspectionRequest, ApiResponse<IEnumerable<InspectionDto>>>
{
  public override void Configure()
  {
    Get("/inspections/all/{warehouseId}");
  }

  public override async Task HandleAsync(GetAllInspectionRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetAllInspectionQuery(req.WarehouseId), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<IEnumerable<InspectionDto>>(result.Value);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync(ct);
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
