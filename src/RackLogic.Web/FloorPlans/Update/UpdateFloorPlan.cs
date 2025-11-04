using System.Text.RegularExpressions;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using RackLogic.UseCases.FloorPlans.Update;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Services;

namespace RackLogic.Web.FloorPlans.Update;

public class UpdateFloorPlan(
  IMediator mediator)
  : Endpoint<RequestUpdateFloorPlan, ApiResponse<bool>>
{
  public override void Configure()
  {
    Put("/floor-plan/update");
    AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    DontThrowIfValidationFails();
    AllowFileUploads();
  }

  public override async Task HandleAsync(RequestUpdateFloorPlan req, CancellationToken ct)
  {
    var form = await HttpContext.Request.ReadFormAsync();

    var files = form.Files.ToList();
    var floorPlanData = ConvertStringDataToModel(req.RequestFloorPlanData);

    if (floorPlanData == null)
    {
      throw new InvalidException(new List<string> { "Invalid JSON data in UpdateFloorPlanData." });
    }

    // Mapping files to FloorAsset
    MapFilesToFloorAssets(floorPlanData.FloorAssets, files);

    var validationResult = await new UpdateFloorPlanRequestValidator().ValidateAsync(floorPlanData);
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await mediator.Send(
        new UpdateFloorPlanCommand(
            User.GetUserId(),
            floorPlanData.WarehouseId,
            floorPlanData.FloorAssets
              .Select(FloorPlanMapper.ToUpdateFloorPlanCommand)
              .ToList()
        ),
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
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }

  private void MapFilesToFloorAssets(List<UpdateFloorPlanInformation>? floorAssets, List<IFormFile>? files)
  {
    if (floorAssets == null || !floorAssets.Any() || files == null || files.Count == 0) return;

    var regex = new Regex(@"files\[(\d+)\]");

    foreach (var file in files)
    {
      var match = regex.Match(file.Name);

      if (!match.Success || !int.TryParse(match.Groups[1].Value, out int index) || index < 0 || index >= floorAssets.Count)
      {
        continue;
      }

      floorAssets[index].Files ??= new List<IFormFile>();
      floorAssets[index].Files!.Add(file);
    }
  }

  private UpdateFloorPlanRequest? ConvertStringDataToModel(string data)
  {
    UpdateFloorPlanRequest? result;
    try
    {
      result = JsonConvert.DeserializeObject<UpdateFloorPlanRequest>(data);
    }
    catch (Exception)
    {
      var jsonString = data.Trim('"').Replace("\n", "");
      result = JsonConvert.DeserializeObject<UpdateFloorPlanRequest>(jsonString);
    }

    return result;
  }
}
