using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.DefectLogs;
using RackLogic.UseCases.DefectLogs.Create;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.DefectLogs.Create;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateDefectLog(IMediator _mediator) : Endpoint<CreateDefectLogCommand, ApiResponse<DefectLogDto>>
{
  public override void Configure()
  {
    Post("/defect-logs/create");
    AllowFileUploads();
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(CreateDefectLogCommand req, CancellationToken ct)
  {
    var validator = new CreateDefectLogValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(req, ct);

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

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
