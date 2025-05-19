using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Inspections.Create;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Inspections.Create;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateInspection(IMediator _mediator) : Endpoint<CreateInspectionCommand, ApiResponse<CreateInspectionDto>>
{
  public override void Configure()
  {
    Post("/inspections/start");
  }

  public override async Task HandleAsync(CreateInspectionCommand req, CancellationToken ct)
  {
    var validator = new CreateInspectionValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(req, ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<CreateInspectionDto>(result.Value);
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
