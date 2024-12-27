using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.GetListComponentBrand;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.GetListComponentBrand;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class GetListComponentBrand(IMediator _mediator) : Endpoint<GetListComponentBrandRequest, PageResponse<GetListComponentBrandResponse>>
{
  public override void Configure()
  {
    Get("/products/brand/{brandId}");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(GetListComponentBrandRequest req, CancellationToken ct)
  {
    //valid data
    var validator = new GetListComponentBrandValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    // Default value if PageNumber or PageSize is not passed
    int pageNumber = req.PageNumber ?? 1;
    int pageSize = req.PageSize ?? 10;

    var result = await _mediator.Send(new GetComponentsByBrandQuery(req.BrandId, pageNumber, pageSize), ct);

    if (result.IsSuccess)
    {
      // Mapper list of ListComponentBrandDTO to list of ListComponentBrandResponse
      var responseList = result.Value.DataDTO
                    .Select(WareHouseMapper.GetComponentBrandRespond)
                    .ToList();
      Response = new PageResponse<GetListComponentBrandResponse>
      {
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalRecords = result.Value.TotalRecords,
        Data = responseList
      };
      return;
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
