using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Users.GetInspectorProfile;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;
using RackLogic.Web.Services;

namespace RackLogic.Web.Users.GetInspectorProfile;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class GetInspectorProfile(IMediator _mediator) : EndpointWithoutRequest<ApiResponse<InspectorProfileDTO>>
{
  public override void Configure()
  {
    Get("/users/getInspectorProfile");
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var userCreatedId = User.GetUserId();
    var result = await _mediator.Send(new GetInspectorProfileQuery(userCreatedId), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<InspectorProfileDTO>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
