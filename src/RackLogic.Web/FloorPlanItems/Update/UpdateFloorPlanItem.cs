using FastEndpoints;
using MediatR;
using RackLogic.UseCases.FloorPlanItems;
using RackLogic.Web.Models;
using RackLogic.UseCases.FloorPlanItems.Update;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ardalis.Result;
using RackLogic.Web.Exceptions;

namespace RackLogic.Web.FloorPlanItems.Update;

public class UpdateFloorPlanItem(IMediator _mediator)
  : Endpoint<UpdateFloorPlanItemRequest, ApiResponse<FloorPlanItemDto>>
{
  public override void Configure()
  {
    Put(UpdateFloorPlanItemRequest.Router);
    AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(UpdateFloorPlanItemRequest req, CancellationToken ct)
  {
    var validator = await new UpdateFloorPlanItemValidator().ValidateAsync(req);

    if (!validator.IsValid)
    {
      var errors = validator.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(
      new UpdateFloorPlanItemCommand
      {
        Id = req.Id,
        BayId = req.BayId!
      },
      ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<FloorPlanItemDto>(result);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }
  }
}
