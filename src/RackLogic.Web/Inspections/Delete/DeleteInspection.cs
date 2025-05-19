using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.Inspections.Delete;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.Delete;

public class DeleteInspection(IMediator _mediator) : Endpoint<DeleteInspectionRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Delete("/inspections/cancel/{Id}");
  }

  public override async Task HandleAsync(DeleteInspectionRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new DeleteInspectionCommand(req.Id), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(true);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    if (result.IsNotFound())
    {
      await SendNotFoundAsync(ct);
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
