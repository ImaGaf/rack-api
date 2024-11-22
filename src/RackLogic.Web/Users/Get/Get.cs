using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Users.Get;
using RackLogic.Web.Models;

namespace RackLogic.Web.Users.Get;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class Get(IMediator _mediator) : EndpointWithoutRequest<ApiResponse<GetUserResponse>>
{
  public override void Configure()
  {
    Get("/users/getUserInformation");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _mediator.Send(new GetUserQuery(User), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<GetUserResponse>(
        new GetUserResponse() 
        {
          FirstName = result.Value.FirstName,
          LastName = result.Value.LastName,
          Email = result.Value.Email,
          PhoneNumber = result.Value.PhoneNumber 
        });
      return;
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
