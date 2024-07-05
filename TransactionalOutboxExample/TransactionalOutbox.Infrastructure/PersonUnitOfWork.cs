using TransactionalOutbox.Core.Infrastructure;

namespace TransactionalOutbox.Infrastructure;

public class PersonUnitOfWork : IPersonUnitOfWork
{
	private readonly PersonDbContext _dbContext;

	public PersonUnitOfWork(PersonDbContext dbcontext, IPersonRepository personRepository)
	{
		_dbContext = dbcontext;
		PersonRepository = personRepository;
	}

	public IPersonRepository PersonRepository { get; }

	public Task SaveChangesAsync(CancellationToken cancellationToken)
	{
		return _dbContext.SaveChangesAsync(cancellationToken);
	}
}
