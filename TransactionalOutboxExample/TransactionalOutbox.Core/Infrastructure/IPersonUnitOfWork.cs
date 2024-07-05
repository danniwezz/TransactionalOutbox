namespace TransactionalOutbox.Core.Infrastructure;
public interface IPersonUnitOfWork
{
	IPersonRepository PersonRepository { get; }
	Task SaveChangesAsync(CancellationToken cancellationToken);
}
