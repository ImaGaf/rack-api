using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Brands.GetOEMBrand;
using RackLogic.Web.Models;
using RackLogic.Web.Warehouses;

namespace RackLogic.Web.Brands.GetOEMBrand;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetOEMBrand(IMediator _mediator) : EndpointWithoutRequest<ApiResponse<List<GetOEMBrandResponse>>>
{
  public override void Configure()
  {
    Get("/brand-OEM/brands");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _mediator.Send(new GetOEMBrandQuery(), ct);

    if (result.IsSuccess)
    {
      var responseList = result.Value.Select(WareHouseMapper.GetGetOEMBrandRespond).ToList();
      Response = new ApiResponse<List<GetOEMBrandResponse>>(responseList);
      return;
    }

    await SendErrorsAsync(500, ct);
  }
}
