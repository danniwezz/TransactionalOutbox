using Shared.TransactionalOutbox.Models;
using StrictId;
using TransactionalOutbox.Core.Models;

namespace TransactionalOutbox.Core.Events;
[EventName("PersonCreated")]
public record PersonCreatedEvent(Id<Person> PersonId, string Name, int Age, ICollection<Address> addresses) : Event;
