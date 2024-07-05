using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StrictId;
using TransactionalOutbox.Core;
using TransactionalOutbox.Core.Infrastructure;
using TransactionalOutbox.Core.Models;
using TransactionalOutbox.Public;

namespace TransactionalOutbox.Api.WebApi;

public static class PersonApi
{
	public static WebApplication MapPersonApi(this WebApplication app)
	{
		var group = app.MapGroup("/api/person")
			.WithOpenApi();
		group.MapGet("", GetPersons);
		group.MapGet("{id}", GetPerson)
			.WithName("GetPerson");
		group.MapPost("", PostPerson)
			.WithName("PostPerson");
		group.MapPut("{id}", PutPerson)
			.WithName("PutPerson");
		return app;
	}

	private static async Task<Ok<List<Person>>> GetPersons(
		IPersonUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		var persons = await unitOfWork.PersonRepository.QueryAsync(x => true, cancellationToken);
		return TypedResults.Ok(persons.ToList());
	}

	private static async Task<Results<Ok<Person>, NotFound>> GetPerson(
		[FromRoute] string id,
		IPersonUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		var personId = new Id<Person>(id);
		var person = await unitOfWork.PersonRepository.FirstOrDefaultAsync(x => x.Id == personId, cancellationToken);
		if (person is not null)
		{
			return TypedResults.Ok(person);
		}
		return TypedResults.NotFound();
	}
	private static async Task<Results<Created<Person>, Conflict>> PostPerson(
		[FromBody] PersonDto personDto,
		IPersonUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		var addresses = personDto.Addresses.Select(x =>
		{
			return Address.Load(
				Id<Address>.NewId(),
				Id<Person>.NewId(),
				(AddressType)x.AddressType,
				x.Street,
				x.City,
				x.State,
				x.ZipCode);
		}).ToList();
		var personEntity = Person.Create(personDto.Name, personDto.Age, addresses);
		unitOfWork.PersonRepository.Add(personEntity);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return TypedResults.Created(personEntity.Id.ToString(), personEntity);

	}

	private static async Task<Results<Ok<Person>, NotFound>> PutPerson(
		[FromBody] PersonDto personDto,
		IPersonUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		var personId = new Id<Person>(personDto.Id);
		var person = await unitOfWork.PersonRepository.SingleOrDefaultAsync(x => x.Id == personId, cancellationToken);

		if (person is null)
		{
			return TypedResults.NotFound();
		}

		person = person.FromDto(personDto);

		await unitOfWork.SaveChangesAsync(cancellationToken);
		return TypedResults.Ok(person);
	}
}
