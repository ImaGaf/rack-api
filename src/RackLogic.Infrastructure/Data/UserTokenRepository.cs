using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Interfaces;

namespace RackLogic.Infrastructure.Data;
public class UserTokenRepository : IUserTokenRepository
{
  private readonly AppDbContext _dbContext;

  public UserTokenRepository(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }
  public async Task DeleteTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
  {
    var tokens = await _dbContext.Set<IdentityUserToken<Guid>>()
        .Where(t => t.UserId == userId)
        .ToListAsync(cancellationToken);

    _dbContext.Set<IdentityUserToken<Guid>>().RemoveRange(tokens);
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
