using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TransactionalOutbox.Core.Infrastructure;

namespace TransactionalOutbox.Infrastructure.Repositories;
public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
	protected readonly DbContext _dbContext;

	public BaseRepository(DbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
		=> await _dbContext.Set<T>().Where(filter).ToListAsync(cancellationToken);

	public async Task<T> SingleAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken) =>
		 await _dbContext.Set<T>().SingleAsync(filter, cancellationToken: cancellationToken);

	public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken) =>
		await _dbContext.Set<T>().SingleOrDefaultAsync(filter, cancellationToken: cancellationToken);

	public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
		=> await _dbContext.Set<T>().FirstOrDefaultAsync(filter, cancellationToken);

	public void Add(T entity) => _dbContext.Set<T>().Add(entity);
}
