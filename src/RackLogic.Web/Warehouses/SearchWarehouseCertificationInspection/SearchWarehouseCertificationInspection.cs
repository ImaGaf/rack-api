using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.SearchWarehouseCertificationInspection;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SearchWarehouseCertificationInspection(IMediator mediator) : Endpoint<SearchWarehouseCertificationInspectionRequest, PageResponse<WarehouseCertificationInspectionDto>>
{
  public override void Configure()
  {
    Get("/inspections/search-certification-inspection");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(SearchWarehouseCertificationInspectionRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(req.ToQuery(), ct);

    if (result.IsSuccess)
    {
      Response = new PageResponse<WarehouseCertificationInspectionDto>
      {
        Data = result.Value.DataDTO,
        PageNumber = result.Value.PageNumber,
        PageSize = result.Value.PageSize,
        TotalRecords = result.Value.TotalRecords
      };

      return;
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
