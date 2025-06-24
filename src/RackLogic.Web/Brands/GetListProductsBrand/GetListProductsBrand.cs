using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Brands.GetListProductsBrand;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Warehouses;

namespace RackLogic.Web.Brands.GetListProductsBrand;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class GetListProductsBrand(IMediator _mediator) : Endpoint<GetListProductsBrandRequest, PageResponse<GetListProductsBrandResponse>>
{
  public override void Configure()
  {
    Get("/brand-OEM/product");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(GetListProductsBrandRequest req, CancellationToken ct)
  {
    //valid data
    var validator = new GetListProductsBrandValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new GetProductsByBrandQuery(req.BrandId, req.WarehouseId, req.IsGetAll, req.Search, req.PageNumber, req.PageSize), ct);

    if (result.IsSuccess)
    {
      // Mapper list of ListComponentBrandDTO to list of ListComponentBrandResponse
      var responseList = result.Value.DataDTO
                    .Select(WareHouseMapper.GetListProductsBrandRespond)
                    .ToList();
      Response = new PageResponse<GetListProductsBrandResponse>
      {
        PageNumber = result.Value.PageNumber,
        PageSize = result.Value.PageSize,
        TotalRecords = result.Value.TotalRecords,
        Data = responseList
      };
      return;
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
