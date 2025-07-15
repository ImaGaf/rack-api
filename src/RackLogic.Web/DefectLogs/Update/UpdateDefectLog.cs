using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.DefectLogs;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using Ardalis.Result;

namespace RackLogic.Web.DefectLogs.Update;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UpdateDefectLog(IMediator _mediator) : Endpoint<UpdateDefectLogRequest, ApiResponse<DefectLogDto>>
{
  public override void Configure()
  {
    Put("/defect-logs/{Id}");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(UpdateDefectLogRequest req, CancellationToken ct)
  {
    var validator = new UpdateDefectLogValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(req.ToCommand(), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<DefectLogDto>(result.Value);
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

    if (result.IsError())
    {
      throw new InvalidException(result?.Errors?.ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
