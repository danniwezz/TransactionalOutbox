using TransactionalOutbox.Core.Infrastructure;
using TransactionalOutbox.Core.Models;

namespace TransactionalOutbox.Infrastructure.Repositories;
public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
	public PersonRepository(PersonDbContext dbContext) : base(dbContext)
	{
	}
}
