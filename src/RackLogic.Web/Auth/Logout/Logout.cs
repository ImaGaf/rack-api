using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Auths.Logout;

namespace RackLogic.Web.Auth.Logout;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class Logout(IMediator _mediator) : EndpointWithoutRequest
{
  public override void Configure()
  {
    Post("/auths/logout");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _mediator.Send(new LogoutCommand(User), ct);

    if (result.IsSuccess)
    {
      await SendNoContentAsync(ct);
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
