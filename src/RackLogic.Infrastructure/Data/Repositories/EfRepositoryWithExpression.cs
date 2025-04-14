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

  public async Task AddToContextAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default)
  {
    await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    if (autoSave)
    {
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
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

  public async Task<List<T>> GetListAsync(Ardalis.Specification.ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

    return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
  }

  public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
  {
    var query = _dbContext.Set<T>().Where(predicate);

    return await query.ToListAsync(cancellationToken);
  }

  public async Task SaveContextAsync(CancellationToken cancellationToken = default)
  {
    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  public async Task SaveEntityAsync(T entity, CancellationToken cancellationToken = default)
  {
    _dbContext.Attach(entity);
    _dbContext.Entry(entity).State = EntityState.Added;
    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  public async Task UpdateToContextAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default)
  {
    _dbContext.Set<T>().Update(entity);

    if (autoSave)
    {
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}
