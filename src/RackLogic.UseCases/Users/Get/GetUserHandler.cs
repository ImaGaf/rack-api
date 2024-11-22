using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.Get;
public class GetUserHandler(UserManager<ApplicationUser> _userManager) : IQueryHandler<GetUserQuery, Result<UserDTO>>
{
  public async Task<Result<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
  {
    var userId = request.User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId is null) return Result.Invalid();

    var user = await _userManager.FindByIdAsync(userId);
    if (user is null) return Result.Invalid();

    return new UserDTO(user.FirstName, user.LastName, user.Email, user.PhoneNumber);
  }
}
