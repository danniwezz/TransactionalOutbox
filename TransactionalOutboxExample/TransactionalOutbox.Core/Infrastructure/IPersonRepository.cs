using TransactionalOutbox.Core.Models;

namespace TransactionalOutbox.Core.Infrastructure;
public interface IPersonRepository : IBaseRepository<Person>
{
	public IQueryable<Person> GetQueryable();
}
