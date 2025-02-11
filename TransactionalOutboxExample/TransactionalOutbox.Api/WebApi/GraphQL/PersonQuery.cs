using TransactionalOutbox.Core.Infrastructure;
using TransactionalOutbox.Public;


namespace TransactionalOutbox.Api.WebApi.GraphQL;

public class PersonQuery
{
	[UseProjection]
	[UseFiltering]
	public IQueryable<PersonDto> GetPersons(IPersonUnitOfWork unitOfWork, CancellationToken cancellationToken)
		=> unitOfWork.PersonRepository.GetQueryable()
		  .Select(person => new PersonDto
		  {
			  Id = person.Id.Value.ToString(),
			  Name = person.Name,
			  Age = person.Age,
			  Addresses = person.Addresses.Select(a => new AddressDto
			  {
				  Id = a.Id.Value.ToString(),
				  PersonId = a.PersonId.Value.ToString(),
				  AddressType = (AddressTypeDto) a.AddressType,
				  Street = a.Street,
				  City = a.City,
				  State = a.State,
				  ZipCode = a.ZipCode
			  }).ToList()
		  });
}




