using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.Web.Exceptions;

namespace RackLogic.Web.Users.Register;

public class Register(IMediator _mediator) : Endpoint<RegisterUserRequest, RegisterUserResponse>
{
  public override void Configure()
  {
    Post("/users/register");
    AllowAnonymous();
    DontThrowIfValidationFails();
  }

  public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
  {
    var validator = new RegisterUserRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }
    var result = await _mediator.Send(UserMapper.ToUserCommand(req), ct);

    if(result.IsSuccess)
    {
      Response = new RegisterUserResponse() { UserId = result.Value};
      return;
    }

    if (result.Status == ResultStatus.Invalid)
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode:500, ct);
  }
}
