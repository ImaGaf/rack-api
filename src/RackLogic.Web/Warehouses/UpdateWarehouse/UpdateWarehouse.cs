using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Warehouses;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.UpdateWarehouse;

public class UpdateWarehouse(IMediator _mediator) : Endpoint<UpdateWarehouseRequest, ApiResponse<WarehouseDto>>
{
  public override void Configure()
  {
    Put("/manager-warehouse/updateWarehouse/{WarehouseId}");
    DontThrowIfValidationFails();
    AllowFileUploads();
  }

  public override async Task HandleAsync(UpdateWarehouseRequest req, CancellationToken ct)
  {
    var validator = new UpdateWarehouseValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(req.ToUpdateWarehouseCommand(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<WarehouseDto>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
