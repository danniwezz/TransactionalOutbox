using Microsoft.EntityFrameworkCore;
using TransactionalOutbox.Core.Infrastructure;
using TransactionalOutbox.Core.Models;

namespace TransactionalOutbox.Infrastructure.Repositories;
public class PersonRepository(PersonDbContext dbContext) : BaseRepository<Person>(dbContext), IPersonRepository
{

	public IQueryable<Person> GetQueryable()
	{
		return dbContext.Person.AsQueryable().AsNoTracking();
	}
}
