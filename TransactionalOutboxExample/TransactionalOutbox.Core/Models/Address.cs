using StrictId;
using TransactionalOutbox.Public;

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

	public Address FromDto(AddressDto dto)
	{
		return new Address
		{
			Id = new Id<Address>(dto.Id),
			PersonId = new Id<Person>(dto.PersonId),
			AddressType = (AddressType)dto.AddressType,
			Street = dto.Street,
			City = dto.City,
			State = dto.State,
			ZipCode = dto.ZipCode
		};
	}

	public AddressDto ToDto()
	{
		return new AddressDto
		{
			Id = Id.ToString(),
			PersonId = PersonId.ToString(),
			AddressType = (AddressTypeDto)AddressType,
			Street = Street,
			City = City,
			State = State,
			ZipCode = ZipCode
		};
	}
}
