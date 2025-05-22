using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.Get;
public class GetUserHandler(
  IRepositoryWithExpression<ApplicationUser> _userManager,
  IUser _user,
  IUserService _userService)
  : IQueryHandler<GetUserQuery, Result<UserDTO>>
{
  public async Task<Result<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
  {
    if (_user.Id is null) return Result.Invalid();

    var user = await _userManager.FirstOrDefaultAsync(x => x.Id == _user.Id, includes: i => i.Parent);
    if (user is null) return Result.Invalid();

    var isPaid = await _userService.IsPaid();
    var isTransactionGenerated = await _userService.IsTransactionGenerated();

    return new UserDTO
      ( user.Id,
        user.FirstName ?? string.Empty,
        user.LastName ?? string.Empty,
        user.Email, user.PhoneNumber,
        user.RegisteredRole ?? string.Empty,
        isPaid,
        isTransactionGenerated
      );
  }
}
