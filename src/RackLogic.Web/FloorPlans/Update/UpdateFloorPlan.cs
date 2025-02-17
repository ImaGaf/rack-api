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
    Put("/floor-plan/update");
    DontThrowIfValidationFails();
    AllowFileUploads();
  }

  public override async Task HandleAsync([FromForm] UpdateFloorPlanRequest req, CancellationToken ct)
  {
    var form = await HttpContext.Request.ReadFormAsync();
    // Parse FloorPlans manually
    req.FloorAsset = form
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
          var title = form[$"{group.Key}.Title"];
          var floor = form[$"{group.Key}.Floor"];
          var description = form[$"{group.Key}.Description"];
          var totalRack = form[$"{group.Key}.totalRack"];
          var totalBay = form[$"{group.Key}.totalBay"];

          var fileUrlsToDelete = form[$"{group.Key}.FileUrlsToDelete"]
              .ToString()
              .Split(',', StringSplitOptions.RemoveEmptyEntries)
              .ToList();
          var newFiles = form.Files.Where(f => f.Name.StartsWith($"{group.Key}.NewFiles")).ToList();
          var filesToUpdate = form.Files.Where(f => f.Name.StartsWith($"{group.Key}.FilesToUpdate")).ToList();

          //Parse Ploor items
          var floorItems = form.Keys
              .Where(k => k.StartsWith($"{group.Key}.Floor[") && !k.Contains(".FloorPlanComment["))
              .GroupBy(k => k.Substring(0, k.LastIndexOf(']') + 1)) // Group by Floor[0], Floor[1], ...
              .Select(itemGroup =>
              {
                var itemId = form[$"{itemGroup.Key}.Id"];
                var rack = form[$"{itemGroup.Key}.Rack"];
                var bay = form[$"{itemGroup.Key}.Bay"];
                var offset = form[$"{itemGroup.Key}.Offset"];
                var size = form[$"{itemGroup.Key}.Size"];
                var width = form[$"{itemGroup.Key}.Width"];
                var height = form[$"{itemGroup.Key}.Height"];
                var objectType = form[$"{itemGroup.Key}.ObjectType"];
                var rotation = form[$"{itemGroup.Key}.Rotation"];

                // Parse Comments in FloorPlanItem
                var comments = form.Keys
                    .Where(c => c.StartsWith($"{itemGroup.Key}.FloorPlanComment["))
                    .GroupBy(c => c.Substring(0, c.LastIndexOf(']') + 1))
                    .Select(commentGroup =>
                    {
                      var commentIdValue = form[$"{commentGroup.Key}.Id"].FirstOrDefault();
                      if (string.IsNullOrEmpty(commentIdValue))
                      {
                        throw new InvalidException(new List<string> { $"Comment Id is missing for {commentGroup.Key}" });
                      }
                      var commentId = Guid.Parse(commentIdValue);

                      return new UpdateFloorPlanComment
                      {
                        Id = commentId,
                        UserName = form[$"{commentGroup.Key}.UserName"].ToString(),
                        Comment = form[$"{commentGroup.Key}.Comment"].ToString(),
                        Date = form[$"{commentGroup.Key}.Date"].ToString()
                      };
                    }).ToList();

                return new UpdateFloorPlanItemInformation
                {
                  Id = Guid.Parse(itemId!),
                  Rack = int.Parse(rack!),
                  Bay = int.Parse(bay!),
                  Offset = offset.FirstOrDefault() ?? string.Empty,
                  Size = int.TryParse(size, out var parsedSize) ? parsedSize : null,
                  Width = double.TryParse(width, out var parsedWidth) ? parsedWidth : null,
                  Height = double.TryParse(height, out var parsedHeight) ? parsedHeight : null,
                  ObjectType = objectType.FirstOrDefault() ?? null,
                  Rotation = double.Parse(rotation!),
                  Comments = comments
                };
              }).ToList();
          return new UpdateFloorPlanInformation
          {
            FloorPlanId = floorPlanId,
            Title = title.ToString(),
            Floor = int.Parse(floor!),
            Description = description.ToString() ?? null,
            TotalRack = int.Parse(totalRack!),
            TotalBay = int.Parse(totalBay!),
            FileUrlsToDelete = fileUrlsToDelete,
            NewFiles = newFiles,
            FilesToUpdate = filesToUpdate,
            Items = floorItems
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
        req.FloorAsset.Select(FloorPlanMapper.ToUpdateFloorPlanCommand).ToList()),
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
