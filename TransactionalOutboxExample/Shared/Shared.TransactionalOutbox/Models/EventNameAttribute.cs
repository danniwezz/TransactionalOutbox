using System.Reflection;

namespace Shared.TransactionalOutbox.Models;
[AttributeUsage(AttributeTargets.Class)]
public class EventNameAttribute : Attribute
{
    public EventNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public static class EventExtensions
{
    // These methods are used by transactional outbox when serializing events.
    public static string GetEventName(this Event @event) => @event.GetType().GetEventName();
    public static string GetEventName(this Type eventType)
    {
        if (!eventType.IsAssignableTo(typeof(Event)))
        {
            throw new Exception($"Type {eventType.FullName} is not an Event");
        }

        return eventType.GetCustomAttribute<EventNameAttribute>()?.Name ?? (eventType.Name.EndsWith("Event") ? eventType.Name.Replace("Event", "") : throw new Exception("Cant figure out event name"));
    }
}
