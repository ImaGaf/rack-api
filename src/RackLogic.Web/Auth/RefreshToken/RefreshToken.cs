using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.Web.Models;

namespace RackLogic.Web.Auth.RefreshToken;

public class RefreshToken(IMediator _mediator) : Endpoint<RefreshTokenRequest, ApiResponse<RefreshTokenResponse>>
{
  public override void Configure()
  {
    Post("/auths/refreshToken");
    AllowAnonymous();
  }

  public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(AuthMapper.ToRefreshTokenCommand(req));

    if (result.IsSuccess)
    {
      Response = new ApiResponse<RefreshTokenResponse>(AuthMapper.ToRefreshTokenResponse(result.Value));
      return;
    }

    if (result.IsInvalid())
    {
      AddError(result.ValidationErrors?.FirstOrDefault()?.ErrorMessage ?? string.Empty);
      await SendErrorsAsync(400, ct);
      return;
    }

    foreach (var error in result.Errors)
    {
      AddError(error);
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
