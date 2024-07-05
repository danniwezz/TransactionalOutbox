namespace Shared.TransactionalOutbox.MessageRelayService;

// Provides a way to notify the message relay service about new messages in the outbox.
public interface IMessageRelayServiceNotifier
{
    Task Notify();
}
