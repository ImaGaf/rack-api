using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Brands.GetAll;
using RackLogic.Web.Models;
using RackLogic.Web.Warehouses;

namespace RackLogic.Web.Brands.GetAll;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetAll(IMediator _mediator) : EndpointWithoutRequest<ApiResponse<List<GetAllResponse>>>
{
  public override void Configure()
  {
    Get("/brand-OEM/getAll");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _mediator.Send(new GetAllBrandQuery(), ct);

    if (result.IsSuccess)
    {
      var responseList = result.Value
                    .Select(WareHouseMapper.GetAllBrandResponse)
                    .ToList();
      Response = new ApiResponse<List<GetAllResponse>>(responseList);
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
