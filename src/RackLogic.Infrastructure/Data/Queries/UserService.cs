using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Users;

namespace RackLogic.Infrastructure.Data.Queries;
public class UserService(
  AppDbContext _context,
  IUser _user)
  : IUserService
{
  public async Task<bool> IsPaid()
  {
    var user = await _context.Users.
      Include(i => i.Parent!)
        .ThenInclude(t => t.Purchase)
      .Include(i => i.Purchase)
      .FirstOrDefaultAsync(x => x.Id == _user.Id);

    var expiryTime = user?.Parent != null ? user?.Parent?.Purchase?.ExpiryTime : user?.Purchase?.ExpiryTime;

    return expiryTime > DateTime.UtcNow;
  }
}
