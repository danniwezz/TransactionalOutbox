using Shared.TransactionalOutbox.Models;
using StrictId;
using System.Collections.ObjectModel;
using TransactionalOutbox.Core.Events;
using TransactionalOutbox.Public;

namespace TransactionalOutbox.Core.Models;

public class Person : AggregateRoot
{
    public Id<Person> Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public ICollection<Address> Addresses { get; set; } = new Collection<Address>();

    public static Person Create(string name, int age, IList<Address> addresses)
    {
		var personId = Id<Person>.NewId();
        var person = new Person
        {
            Id = Id<Person>.NewId(),
            Name = name,
            Age = age,
            Addresses = addresses.Select(x =>
			{
				return Address.Create(
					personId,
					(AddressType)x.AddressType,
					x.Street,
					x.City,
					x.State,
					x.ZipCode);
			}).ToList()
		};
        person.AddEvent(new PersonCreatedEvent(person.Id, name, age, addresses));
        return person;
    }

    public static Person Load(Id<Person> id, string name, int age, IList<Address> addresses)
    {
        return new Person
        {
            Id = id,
            Name = name,
            Age = age,
            Addresses = addresses
        };
    }

	public Person FromDto(PersonDto personDto)
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

    public PersonDto ToDto()
	{
		return new PersonDto
		{
			Id = Id.ToString(),
			Name = Name,
			Age = Age,
			Addresses = Addresses.Select(x => x.ToDto()).ToList()
		};
	}
}
