using StrictId;

namespace TransactionalOutbox.Core.Models;
public class Address
{
    public Id<Address> Id { get; set; }
    public Id<Person> PersonId { get; set; }
	public AddressType AddressType { get; set; }
    public string Street { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string State { get; set; } = string.Empty;
	public string ZipCode { get; set; } = string.Empty;

	public static Address Create(Id<Person> personId, AddressType addressType, string street, string city, string state, string zipCode)
	{
		return new Address
		{
			Id = Id<Address>.NewId(),
			PersonId = personId,
			AddressType = addressType,
			Street = street,
			City = city,
			State = state,
			ZipCode = zipCode
		};
	}

	public static Address Load(Id<Address> id, Id<Person> personId, AddressType addressType, string street, string city, string state, string zipCode)
	{
		return new Address
		{
			Id = id,
			PersonId = personId,
			AddressType = addressType,
			Street = street,
			City = city,
			State = state,
			ZipCode = zipCode
		};
	}
}
