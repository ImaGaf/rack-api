using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Inspections.Update;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Inspections.Create;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.Update;

public class UpdateInspection(IMediator _mediator) : Endpoint<UpdateInspectionCommand, ApiResponse<InspectionDto>>
{
  public override void Configure()
  {
    Put("/inspections/update");
  }

  public override async Task HandleAsync(UpdateInspectionCommand req, CancellationToken ct)
  {
    var validator = new CreateInspectionValidator();

    var result = await _mediator.Send(req, ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<InspectionDto>(result.Value);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
