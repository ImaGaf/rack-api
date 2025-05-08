using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Inspections;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.Get;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetInspectionById(IMediator _mediator) : Endpoint<GetInspectionByIdRequest, ApiResponse<InspectionDto>>
{
  public override void Configure()
  {
    Get("/inspections/getById/{InspectionId}");
  }

  public override async Task HandleAsync(GetInspectionByIdRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(req.ToQuery(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<InspectionDto>(result.Value);
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
