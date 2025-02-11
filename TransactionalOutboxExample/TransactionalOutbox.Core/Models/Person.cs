using Shared.TransactionalOutbox.Models;
using StrictId;
using System.Collections.ObjectModel;
using TransactionalOutbox.Core.Events;

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
            Id = personId,
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
}
