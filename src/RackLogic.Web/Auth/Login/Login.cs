using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Auth.Login;

public class Login(IMediator _mediator) : Endpoint<LoginRequest, ApiResponse<LoginResponse>>
{
  public override void Configure()
  {
    Post("/auths/login");
    AllowAnonymous();
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
  {
    var validator = new LoginRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(AuthMapper.ToLoginCommand(req));

    if (result.IsSuccess)
    {
      Response = new ApiResponse<LoginResponse>(AuthMapper.ToLoginResponse(result.Value));
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    if (result.IsForbidden())
    {
      throw new ForbiddenException(result?.Errors?.FirstOrDefault());
    }

    foreach (var error in result.Errors)
    {
      AddError(error);
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
