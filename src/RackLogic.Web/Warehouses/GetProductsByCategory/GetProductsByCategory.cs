using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.GetProductsByCategory;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.GetProductsByCategory;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetProductsByCategory(IMediator _mediator) : Endpoint<GetProductsByCategoryRequest, ApiResponse<List<GetProductsByCategoryResponse>>>
{
  public override void Configure()
  {
    Get("/products/by-category");
    DontThrowIfValidationFails();
  }
  public override async Task HandleAsync(GetProductsByCategoryRequest req, CancellationToken ct)
  {
    var validator = new GetProductsByCategoryValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new GetProductsByCategoryQuery(req.ManufacturerId, req.Category), ct);

    if (result.IsNotFound())
    {
      throw new NotFoundException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsSuccess)
    {
      var responseList = result.Value.Select(WareHouseMapper.GetProductByCategoryRespond).ToList();
      Response = new ApiResponse<List<GetProductsByCategoryResponse>>(responseList);
      return;
    }
    await SendErrorsAsync(500, ct);
  }
}
