namespace TransactionalOutbox.Public;
public class PersonDto
{
	public string Id { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public int Age { get; set; }
	public IList<AddressDto> Addresses { get; set; } = new List<AddressDto>();

}

public class AddressDto
{
	public string Id { get; set; } = string.Empty;
	public string PersonId { get; set; } = string.Empty;
	public AddressTypeDto AddressType { get; set; }
	public string Street { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string State { get; set; } = string.Empty;
	public string ZipCode { get; set; } = string.Empty;
}

public enum AddressTypeDto
{
	Unknown = 0,
	Home,
	Work,
	Other
}
