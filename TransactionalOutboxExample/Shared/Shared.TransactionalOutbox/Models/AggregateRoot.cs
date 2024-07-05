namespace Shared.TransactionalOutbox.Models;

public abstract class AggregateRoot : IEventSource
{
    IEnumerable<Event> IEventSource.Events => _events.ToList();
    void IEventSource.ClearEvents() => _events.Clear();
    private readonly List<Event> _events = new();
    protected void AddEvent(Event @event) => _events.Add(@event);
}