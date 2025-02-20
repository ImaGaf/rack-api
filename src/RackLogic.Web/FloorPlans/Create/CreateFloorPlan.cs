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

    req.FloorAsset = await GetDataCreateFloorPlans(form);

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
        req.FloorAsset.Select(FloorPlanMapper.ToFloorPlanCommand).ToList()),
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

  public Task<List<FloorPlanRequest>> GetDataCreateFloorPlans(IFormCollection form)
  {
    var floorPlans = form
        .Keys
        .Where(key => key.StartsWith("FloorPlans["))
        .GroupBy(key => key.Substring(0, key.IndexOf(']') + 1)) // Group by FloorPlans[0], FloorPlans[1], ...
        .Select(group =>
        {
          var floorPlanRequestId = form[$"{group.Key}.FloorPlanRequestId"];
          var assetName = form[$"{group.Key}.AssetName"];
          var floorNumber = form[$"{group.Key}.FloorNumber"];
          var description = form[$"{group.Key}.Description"];
          var totalRack = form[$"{group.Key}.totalRack"];
          var totalBay = form[$"{group.Key}.totalBay"];
          var files = form.Files.Where(f => f.Name.StartsWith($"{group.Key}.Files")).ToList();

          // Parse Floor items
          var floorItems = form.Keys
              .Where(k => k.StartsWith($"{group.Key}.Floor[") && !k.Contains(".FloorPlanComment["))
              .GroupBy(k => k.Substring(0, k.LastIndexOf(']') + 1)) // Group by Floor[0], Floor[1], ...
                        .Select(floorGroup =>
                        {
                          var floorPlanItemId = form[$"{floorGroup.Key}.Id"];
                          var rack = form[$"{floorGroup.Key}.Rack"];
                          var bay = form[$"{floorGroup.Key}.Bay"];
                          var offset = form[$"{floorGroup.Key}.Offset"];
                          var size = form[$"{floorGroup.Key}.Size"];
                          var width = form[$"{floorGroup.Key}.Width"];
                          var height = form[$"{floorGroup.Key}.Height"];
                          var objectType = form[$"{floorGroup.Key}.ObjectType"];
                          var rotation = form[$"{floorGroup.Key}.Rotation"];

                          // Parse FloorPlanComments
                          var comments = form.Keys
                            .Where(c => c.StartsWith($"{floorGroup.Key}.FloorPlanComment["))
                            .Distinct()
                            .GroupBy(c => c.Substring(0, c.LastIndexOf(']') + 1)) // Group by FloorPlanComment[0], ...
                            .Select(commentGroup => new Comment
                            {
                              userName = form[$"{commentGroup.Key}.UserName"].ToString(),
                              comment = form[$"{commentGroup.Key}.Comment"].ToString(),
                              Id = Guid.TryParse(form[$"{commentGroup.Key}.Id"], out var commentId) ? commentId : Guid.NewGuid()
                            }).ToList();

                          return new FloorPlanItem
                          {
                            Id = Guid.TryParse(floorPlanItemId, out var itemId) ? itemId : Guid.NewGuid(),
                            Rack = int.Parse(rack!),
                            Bay = int.Parse(bay!),
                            Offset = offset.FirstOrDefault() ?? string.Empty,
                            Size = int.TryParse(size, out var parsedSize) ? parsedSize : null,
                            Width = double.TryParse(width, out var parsedWidth) ? parsedWidth : null,
                            Height = double.TryParse(height, out var parsedHeight) ? parsedHeight : null,
                            ObjectType = objectType.FirstOrDefault() ?? null,
                            Rotation = double.TryParse(rotation, out var parsedRotation) ? parsedRotation : 0,
                            Comments = comments
                          };
                        }).ToList();

          return new FloorPlanRequest
          {
            FloorPlanRequestId = Guid.TryParse(floorPlanRequestId, out var requestId) ? requestId : Guid.NewGuid(),
            Title = assetName.ToString(),
            Floor = int.Parse(floorNumber!),
            Description = description.ToString() ?? null,
            TotalRack = int.Parse(totalRack!),
            TotalBay = int.Parse(totalBay!),
            Items = floorItems,
            Files = files.ToList()
          };
        }).ToList();

    return Task.FromResult(floorPlans);
  }
}
