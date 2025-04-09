using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.Web.Brands.GetProductsByCategory;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Warehouses;

namespace RackLogic.Web.BayConfigurator.CreateProduct;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateCustomComponent(IMediator _mediator) : Endpoint<CreateCustomComponentRequest, ApiResponse<GetProductsByCategoryResponse>>
{
  public override void Configure()
  {
    Post("/bay-configurators/createCustomComponent");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(CreateCustomComponentRequest req, CancellationToken ct)
  {
    var validator = new CreateCustomComponentValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(req.ToCreateProductCommand(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<GetProductsByCategoryResponse>(result.Value.ToCreateProductResponse());
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
