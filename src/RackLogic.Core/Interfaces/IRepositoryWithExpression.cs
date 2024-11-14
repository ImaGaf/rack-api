using System.Linq.Expressions;
using Ardalis.SharedKernel;

namespace RackLogic.Core.Interfaces;
public interface IRepositoryWithExpression<T> : IRepository<T> where T : class, IAggregateRoot
{
  Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
  Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
  Task DeleteAsync(Expression<Func<T, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default(CancellationToken));
}
