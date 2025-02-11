using StrictId;
using TransactionalOutbox.Core;
using TransactionalOutbox.Core.Models;
using TransactionalOutbox.Public;

namespace TransactionalOutbox.Api.Mappers;

public static class PersonMapper
{
	public static PersonDto ToDto(this Person person)
	{
		return new PersonDto
		{
			Id = person.Id.Value.ToString(),
			Name = person.Name,
			Age = person.Age,
			Addresses = person.Addresses.Select(x => new AddressDto
			{
				Id = x.Id.Value.ToString(),
				PersonId = x.PersonId.Value.ToString(),
				AddressType = (AddressTypeDto)x.AddressType,
				Street = x.Street,
				City = x.City,
				State = x.State,
				ZipCode = x.ZipCode
			}).ToList()
		};
	}
	public static Person ToEntity(this PersonDto personDto)
	{
		return new Person
		{
			Id = new Id<Person>(personDto.Id),
			Name = personDto.Name,
			Age = personDto.Age,
			Addresses = personDto.Addresses.Select(x => new Address
			{
				Id = new Id<Address>(x.Id),
				PersonId = new Id<Person>(x.PersonId),
				AddressType = (AddressType)x.AddressType,
				Street = x.Street,
				City = x.City,
				State = x.State,
				ZipCode = x.ZipCode
			}).ToList()
		};
	}
}
