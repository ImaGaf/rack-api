using System.Linq.Expressions;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Interfaces;

namespace RackLogic.Infrastructure.Data.Repositories;
public class EfRepositoryWithExpression<T> : EfRepository<T>, IRepositoryWithExpression<T> where T : class, IAggregateRoot
{
  private readonly AppDbContext _dbContext;
  public EfRepositoryWithExpression(AppDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task DeleteAsync(Expression<Func<T, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
  {
    var entities = await _dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
    _dbContext.Set<T>().RemoveRange(entities);

    if (autoSave)
    {
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object?>>[] includes)
  {
    var query = _dbContext.Set<T>().Where(predicate);

    if (includes is not null)
    {
      foreach (var include in includes)
      {
        query = query.Include(include);
      }
    }

    return await query.ToListAsync(cancellationToken);
  }

  public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object?>>[] includes)
  {
    var query = _dbContext.Set<T>().Where(predicate);

    if (includes is not null)
    {
      foreach (var include in includes)
      {
        query = query.Include(include);
      }
    }

    return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
  }
}
