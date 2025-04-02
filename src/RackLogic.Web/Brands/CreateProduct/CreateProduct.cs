using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.Web.Brands.GetProductsByCategory;
using RackLogic.Web.Warehouses;

namespace RackLogic.Web.Brands.CreateProduct;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateProduct(IMediator _mediator) : Endpoint<CreateProductRequest, GetProductsByCategoryResponse>
{
  public override void Configure()
  {
    Post("/brand-OEM/Create");
  }

  public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(req.ToCreateProductCommand(), ct);

    if (result.IsSuccess)
    {
      Response = result.Value.ToCreateProductResponse();
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
