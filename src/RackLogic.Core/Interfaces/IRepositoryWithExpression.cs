using System.Linq.Expressions;
using Ardalis.SharedKernel;

namespace RackLogic.Core.Interfaces;
public interface IRepositoryWithExpression<T> : IRepository<T> where T : class, IAggregateRoot
{
  Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<T, object?>>[] includes);
  Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<T, object?>>[] includes);
  Task DeleteAsync(Expression<Func<T, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default(CancellationToken));
  Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
  Task AddToContextAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default(CancellationToken));
  Task SaveContextAsync(CancellationToken cancellationToken = default);
  Task UpdateToContextAsync(T entity, bool autoSave = false, CancellationToken cancellationToken = default);
  Task SaveEntityAsync(T entity, CancellationToken cancellationToken = default);
}
