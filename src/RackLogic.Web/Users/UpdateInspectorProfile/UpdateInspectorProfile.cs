using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Services;

namespace RackLogic.Web.Users.UpdateInspectorProfile;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UpdateInspectorProfile(IMediator _mediator) : Endpoint<UpdateInspectorProfileRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/users/inspectorProfile");
    AllowFileUploads();
  }
  public override async Task HandleAsync(UpdateInspectorProfileRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();
    var userRegisterRole = User.GetUserRegisteredRole();

    var result = await _mediator.Send(req.ToInspectorProfileCommand(userRegisterRole), ct);

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
