using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.FloorPlans.Get;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlans.Get;

public class GetFloorPlanById(IMediator _mediator)
  : Endpoint<GetFloorPlanByIdRequest, ApiResponse<List<GetFloorPlanByIdResponse>>>
{
  public override void Configure()
  {
    Post("/floor-plan/getById/{WarehouseId}");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(GetFloorPlanByIdRequest req, CancellationToken ct)
  {
    var validator = new GetFloorPlanByIdRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(
      new GetFloorPlanByIdQuery(
        req.WarehouseId),
      ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<List<GetFloorPlanByIdResponse>>(result.Value.Select(FloorPlanMapper.ToGetFloorPlanByIdResponse).ToList());
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
