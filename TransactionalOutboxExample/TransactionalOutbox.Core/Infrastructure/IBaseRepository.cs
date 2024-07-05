using System.Linq.Expressions;

namespace TransactionalOutbox.Core.Infrastructure;
public interface IBaseRepository<T>
{
	Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
	Task<T> SingleAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
	Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
	Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);

	void Add(T entity);

}
