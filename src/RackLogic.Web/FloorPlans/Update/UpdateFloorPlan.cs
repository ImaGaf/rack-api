using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RackLogic.UseCases.FloorPlans.Update;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.FloorPlans.Update;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UpdateFloorPlan(IMediator _mediator) : Endpoint<UpdateFloorPlanRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/floor-plan/update");
    DontThrowIfValidationFails();
    AllowFileUploads();
  }

  public override async Task HandleAsync([FromForm] UpdateFloorPlanRequest req, CancellationToken ct)
  {
    var form = await HttpContext.Request.ReadFormAsync();
    // Parse FloorPlans manually
    req.FloorPlans = form
        .Keys
        .Where(key => key.StartsWith("FloorPlans["))
        .GroupBy(key => key.Substring(0, key.IndexOf(']') + 1)) // Group by FloorPlans[0], FloorPlans[1], ...
        .Select(group =>
        {
          //Get the first value in String Values ​​as FloorPlanId
          var floorPlanIdValue = form[$"{group.Key}.FloorPlanId"].FirstOrDefault(); 
          if (string.IsNullOrEmpty(floorPlanIdValue))
          {
            var errors = new List<string>
              {
                $"FloorPlanId is missing for {group.Key}"
              };
            throw new InvalidException(errors);
          }
          var floorPlanId = Guid.Parse(floorPlanIdValue);

          var assetName = form[$"{group.Key}.AssetName"];
          var floor = form[$"{group.Key}.Floor"];
          var fileUrlsToDelete = form[$"{group.Key}.FileUrlsToDelete"]
              .ToString()
              .Split(',', StringSplitOptions.RemoveEmptyEntries)
              .ToList();
          var newFiles = form.Files.Where(f => f.Name.StartsWith($"{group.Key}.NewFiles")).ToList();
          var filesToUpdate = form.Files.Where(f => f.Name.StartsWith($"{group.Key}.FilesToUpdate")).ToList();

          return new UpdateFloorPlanInformation
          {
            FloorPlanId = floorPlanId,
            AssetName = assetName.ToString(),
            Floor = floor.ToString(),
            FileUrlsToDelete = fileUrlsToDelete,
            NewFiles = newFiles,
            FilesToUpdate = filesToUpdate
          };
        }).ToList();

    var validator = new UpdateFloorPlanRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(
      new UpdateFloorPlanCommand(
        User,
        req.WarehouseId,
        req.FloorPlans.Select(FloorPlanMapper.ToUpdateFloorPlanCommand).ToList()),
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
