using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.Web.Models;

namespace RackLogic.Web.Auth.Login;

public class Login(IMediator _mediator) : Endpoint<LoginRequest, ApiResponse<LoginResponse>>
{
  public override void Configure()
  {
    Post("/auths/login");
    AllowAnonymous();
  }

  public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(AuthMapper.ToLoginCommand(req));

    if (result.IsSuccess)
    {
      Response = new ApiResponse<LoginResponse>(AuthMapper.ToLogiResponse(result.Value));
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
