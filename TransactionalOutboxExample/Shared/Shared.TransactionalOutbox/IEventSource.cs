using Shared.TransactionalOutbox.Models;

namespace Shared.TransactionalOutbox;

public interface IEventSource
{
	IEnumerable<Event> Events { get; }
	void ClearEvents();
}