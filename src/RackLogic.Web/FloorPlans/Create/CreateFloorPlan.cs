using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlans.Create;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateFloorPlan(IMediator _mediator) : Endpoint<CreateFloorPlanRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/floor-plan/create");
    DontThrowIfValidationFails();
    AllowFileUploads();
  }

  public override async Task HandleAsync([FromForm] CreateFloorPlanRequest req, CancellationToken ct)
  {
    var form = await HttpContext.Request.ReadFormAsync();
    // Parse FloorPlans manually
    req.FloorPlans = form
    .Keys
    .Where(key => key.StartsWith("FloorPlans["))
    .GroupBy(key => key.Substring(0, key.IndexOf(']') + 1)) // Group by FloorPlans[0], FloorPlans[1], ...
    .Select(group =>
    {
      var assetName = form[$"{group.Key}.AssetName"];
      var floor = form[$"{group.Key}.Floor"];
      var files = form.Files.Where(f => f.Name.StartsWith($"{group.Key}.Files")).ToList();

      return new FloorPlanRequest
      {
        AssetName = assetName.ToString(),
        Floor = floor.ToString(),
        Files = files.ToList()
      };
    }).ToList();

    var validator = new CreateFloorPlanRequestValidator();
    var validationResult = validator.Validate(req);
        
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(
      new CreateFloorPlanCommand(
        User, req.WarehouseId,
        req.FloorPlans.Select(FloorPlanMapper.ToFloorPlanCommand).ToList()),
      ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
      return;
    }

    if (result.IsUnauthorized())
    {
      await SendUnauthorizedAsync();
      return;
    }

    if (result.IsForbidden())
    {
      await SendForbiddenAsync();
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
